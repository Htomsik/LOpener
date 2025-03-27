using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UICore.Models;

namespace UICoreTest;


internal class Data
{
    public string AppName = "App";

    private readonly string _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public readonly string RemoteDirectory;
    public readonly string RemoteUpdateFilePath;
    public readonly string RemoteUpdateArchiveFilePath;

    public readonly string AppDirectory;
    public readonly string AppExePath;
    public readonly string AppUpdateFilePath;

    public Data()
    {
        RemoteDirectory = Path.Combine(_tempDirectory, "Remote");
        RemoteUpdateFilePath = Path.Combine(RemoteDirectory, "update.json");
        RemoteUpdateArchiveFilePath = Path.Combine(RemoteDirectory, "update.zip");

        AppDirectory = Path.Combine(_tempDirectory, AppName);
        AppExePath = Path.Combine(_tempDirectory, $"{AppName}.exe");
        AppUpdateFilePath = Path.Combine(AppDirectory, "update.json");
    }
    
    public Settings CreateSettings(string? appName = null, 
        string? exePath = null, 
        string? appDirectory = null,
        string? remoteDirectory = null,
        int? updateDelaySeconds = null)
    {
        return new Settings(
            appName ?? AppName,
            appName ?? AppName,
            exePath ?? AppExePath,
            appDirectory ?? AppDirectory,
            updateDelaySeconds ?? 10,
            new SyncSettings(SyncSettingsType.Directory, 
                remoteDirectory ?? RemoteDirectory, 
                "update.json", 
                "update.zip"
                )
        );
    }
    
    public IConfiguration CreateConfiguration(string? appName = null, 
        string? defaultApp = null,
        string? availableAppName = null,
        string? exePath = null, 
        string? appDirectory = null,
        string? remoteDirectory = null)
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "CurrentApp", appName ?? AppName },
            { "DefaultApp", defaultApp ?? AppName},
            { "AvailableApps:0:Title", availableAppName ?? AppName },
            { "AvailableApps:0:Parameter", availableAppName ?? AppName },
            { "AvailableApps:0:ExePath", exePath ?? AppExePath },
            { "AvailableApps:0:DirectoryPath", appDirectory ?? AppDirectory },
            { "AvailableApps:0:UpdateDelaySeconds", "10" },
            { "AvailableApps:0:Sync:Type", "Directory" },
            { "AvailableApps:0:Sync:Path", remoteDirectory ?? RemoteDirectory },
            { "AvailableApps:0:Sync:UpdateFileName", "update.json" },
            { "AvailableApps:0:Sync:UpdateArchiveFileName", "update.zip" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
    
    
    public void CreateTempData(UpdaterInfo? appUpdaterInfo = null, UpdaterInfo? remoteUpdaterInfo = null)
    {
        DeleteTempData();
        
        var remoteUpdateContent = remoteUpdaterInfo != null 
            ? JsonConvert.SerializeObject(remoteUpdaterInfo)
            : "{}";
        
        var appUpdateContent = appUpdaterInfo != null 
            ? JsonConvert.SerializeObject(appUpdaterInfo)
            : "{}";
        
        Directory.CreateDirectory(RemoteDirectory);
        File.WriteAllText(RemoteUpdateFilePath, remoteUpdateContent);
        File.WriteAllText(RemoteUpdateArchiveFilePath, "no content");
    
        Directory.CreateDirectory(AppDirectory);
        File.WriteAllText(AppUpdateFilePath, appUpdateContent);
        File.WriteAllText(AppExePath, "{}");
    }
    
    public void DeleteTempData()
    {
        if(Directory.Exists(RemoteDirectory))
            Directory.Delete(RemoteDirectory, true);
        
        if (Directory.Exists(AppDirectory))
            Directory.Delete(AppDirectory, true);
    }
}