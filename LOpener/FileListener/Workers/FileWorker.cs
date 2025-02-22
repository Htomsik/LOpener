using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text.Json;
using FileListener.Models;
using FileListener.Services;

namespace FileListener.Workers;

public sealed class FileWorker(ILogger<FileWorker> logger, ISettingsService settingsService)
    : BaseWorker(logger, settingsService)
{
    /// <summary>
    ///     Process one operation unit (exclude logging) 
    /// </summary>
    protected override async Task ProcessAsync(CancellationToken stoppingToken)
    {
        var newParameters = await ReadParametersAsync(stoppingToken);
        if (newParameters.Count == 0)
        {
            Logger.LogWarning("No files for generate update file were provided.");
            return;
        }
        
        var updaterInfo = new UpdaterInfo(SettingsService.Settings.AppName,newParameters);
        
        GenerateArchive(updaterInfo.FileParameters, stoppingToken);
        await GenerateUpdateFileAsync(updaterInfo, stoppingToken);
    }

    /// <summary>
    ///     Generate Update File from file parameters
    /// </summary>
    private async Task GenerateUpdateFileAsync(UpdaterInfo updaterInfo, CancellationToken stoppingToken)
    {
        if (updaterInfo.FileParameters.Count == 0)
        {
            Logger.LogWarning("No files for generate update file were provided.");
            return;
        }
        
        if (string.IsNullOrEmpty(updaterInfo.AppName))
        {
            Logger.LogError("{name} is empty.", nameof(updaterInfo.AppName));
            return;
        }
        
        var json = JsonSerializer.Serialize(updaterInfo, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        var filePath = Path.Combine(SettingsService.Settings.UpdateDirectoryPath,SettingsService.Settings.UpdateFileName);
        try
        {
            await File.WriteAllTextAsync(filePath, json, stoppingToken);
        }
        catch (Exception e)
        {
           Logger.LogError(e, "Error writing file: {path}",  filePath);
           return;
        }
        
        Logger.LogInformation("File was created: {name}", filePath);
    }
    
    /// <summary>
    ///     Move updated files to archive
    /// </summary>
    private void GenerateArchive(ICollection<FileParameter> fileParameters, CancellationToken stoppingToken)
    {
        if (fileParameters.Count == 0)
        {
            Logger.LogWarning("No files for generate archive file were provided.");
            return;
        }
        
        var archivePath = Path.Combine(SettingsService.Settings.UpdateDirectoryPath,
            SettingsService.Settings.UpdateArchiveFileName);

        try
        {
            if (File.Exists(archivePath))
            {
                File.Delete(archivePath);
            }

            using var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
            foreach (var file in fileParameters)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                archive.CreateEntryFromFile(file.FilePath, file.FileName);
            }
        }
        catch (Exception e)
        {
            Logger.LogError("Can't create archive. {error}", e);
            return;
        }
        
        Logger.LogInformation("Archive was created: {name}", archivePath);
    }
    
    /// <summary>
    ///     Read parameters from files and generate needed info for updates
    /// </summary>
    private async Task<ICollection<FileParameter>> ReadParametersAsync(CancellationToken stoppingToken)
    {
        var fileParameters = new ConcurrentBag<FileParameter>();
        if (stoppingToken.IsCancellationRequested)
        {
            return fileParameters.ToList();
        }
        
        // Read for all Types
        var fileTypes = new ConcurrentBag<string>(SettingsService.Settings.FileFormats);
        var tasks = fileTypes.Select(async item =>
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            
            await Task.Run(() =>
            {
                var filePaths = GetFiles(item);
                var parameters = ReadFileParameters(filePaths,stoppingToken);
                
                foreach (var fileParameter in parameters) 
                    fileParameters.Add(fileParameter);
                
            }, stoppingToken);
        });
        await Task.WhenAll(tasks);
        
        Logger.LogInformation("{GenerateParametersAsync} Finished. Files to process count: {count}", nameof(ReadParametersAsync), fileParameters.Count);
        
        return fileParameters.ToList();
    }

    /// <summary>
    ///     Get files for specific format
    /// </summary>
    private IEnumerable<string> GetFiles(string fileFormat)
    {
        return Directory.GetFiles(SettingsService.Settings.UpdateDirectoryPath, $"*.{fileFormat}", SearchOption.TopDirectoryOnly);
    }

    /// <summary>
    ///     Read needed for update parameters from files
    /// </summary>
    private  IEnumerable<FileParameter> ReadFileParameters(IEnumerable<string> files,CancellationToken stoppingToken)
    {
        var fileParameters = new List<FileParameter>();
        foreach (var filePath in files)
        {
            if (stoppingToken.IsCancellationRequested)
                return fileParameters;

            if (!File.Exists(filePath))
            {
                Logger.LogWarning("{filePath} not found", filePath);
                continue;
            }
            
            var fileInfo = new FileInfo(filePath);
            var fileParameter = new FileParameter(fileInfo.Name, fileInfo.LastWriteTimeUtc, fileInfo.FullName);
            
            fileParameters.Add(fileParameter);
        }
        
        return fileParameters;
    }
}