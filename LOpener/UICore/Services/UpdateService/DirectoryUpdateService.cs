using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UICore.Models;
using UICore.Services.SettingsService;

namespace UICore.Services.UpdateService;

/// <summary>
///     Update files from target local directory
/// </summary>
public sealed class DirectoryUpdateService(ILogger<DirectoryUpdateService> logger, ISettingsService settingsService)
    : IUpdateService
{
    public void Update()
    {
        if (!NeedUpdate())
        {
            logger.LogInformation("[{AppName}] Update doesn't need", settingsService.Settings?.Parameter);
            return;
        }
        
        try
        {
            using var archive = ZipFile.Open(settingsService.Settings?.Sync.UpdateArchiveFilePath, ZipArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                var filePath = Path.Combine(settingsService.Settings?.DirectoryPath!, entry.FullName);
                entry.ExtractToFile(filePath, true);
            }
        }
        catch (Exception e)
        {
            logger.LogError("[{AppName}] Can't decompress update archive: {message}", settingsService.Settings?.Parameter, e.Message);
        }
        
        logger.LogInformation("[{AppName}] Update complete", settingsService.Settings?.Parameter);
    }

    public  bool NeedUpdate()
    {
        if (!CanUpdate())
        {
            return false;
        }
        
        // IF update info file doesn't exist, just create this
        if (!File.Exists(settingsService.Settings?.UpdateFilePath))
        {
            return true;
        }

        UpdaterInfo? remoteUpdaterInfo;
        UpdaterInfo? appUpdaterInfo;
        
        try
        {
            var appUpdateJson = File.ReadAllText(settingsService.Settings?.UpdateFilePath!);
            appUpdaterInfo = JsonConvert.DeserializeObject<UpdaterInfo>(appUpdateJson);

            var remoteUpdateJson = File.ReadAllText(settingsService.Settings?.UpdateFilePath!);
            remoteUpdaterInfo = JsonConvert.DeserializeObject<UpdaterInfo>(remoteUpdateJson);
        }
        catch (Exception e)
        {
            logger.LogError("[{AppName}] Can't Deserialize update info: {Message}",settingsService.Settings?.Parameter, e.Message);
            return false;
        }
        
        if (remoteUpdaterInfo is null)
        {
            logger.LogWarning("[{AppName}] Remote update info failed to load",settingsService.Settings?.Parameter);
            return false;
        }

        if (appUpdaterInfo is null)
        {
            logger.LogWarning("[{AppName}] App update info failed to load", settingsService.Settings?.Parameter);
            return true;   
        }

        return appUpdaterInfo.Equals(remoteUpdaterInfo);
    }
    
    public bool CanUpdate()
    {
        if (!Directory.Exists(settingsService.Settings?.Sync.Path))
        {
            logger.LogError("[{AppName}] Remote update directory {file} does not exist", settingsService.Settings?.Parameter, settingsService.Settings?.Sync.Path);
            return false;
        }
        
        if (!File.Exists(settingsService.Settings?.Sync.UpdateFilePath))
        {
            logger.LogError("[{AppName}] Remote update file {file} does not exist", settingsService.Settings?.Parameter, settingsService.Settings?.Sync.UpdateFilePath);
            return false;
        }
        
        if (!File.Exists(settingsService.Settings?.Sync.UpdateArchiveFilePath))
        {
            logger.LogError("[{AppName}] Remote update archive {file} does not exist", settingsService.Settings?.Parameter, settingsService.Settings?.Sync.UpdateArchiveFilePath);
            return false;
        }

        if (!Directory.Exists(settingsService.Settings?.DirectoryPath))
        {
            logger.LogError("[{AppName}] App directory {file} does not exist", settingsService.Settings?.Parameter,  settingsService.Settings?.DirectoryPath);
            return false;
        }
        
        if (!Directory.Exists(settingsService.Settings?.DirectoryPath))
        {
            logger.LogError("[{AppName}] App update file {file} does not exist", settingsService.Settings?.Parameter, settingsService.Settings?.UpdateFilePath);
            return false;
        }
        
        return true;
    }
}