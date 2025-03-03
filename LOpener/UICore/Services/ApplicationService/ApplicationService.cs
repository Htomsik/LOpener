using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UICore.Services.SettingsService;
using UICore.Services.StatusService;

namespace UICore.Services.ApplicationService;

public class ApplicationService(ILogger<ApplicationService> logger,
    ISettingsService settingsService, 
    IStatusService statusService) : IApplicationService
{
    public event Action? ShutdownRequested;
    
    public async Task<bool> LaunchTarget()
    {
        if (!File.Exists(settingsService.Settings?.ExePath))
        {
            logger.LogError("[{application}] exe file not found: {exe path}", settingsService.Settings!.Parameter, settingsService.Settings!.ExePath);
            await statusService.ChangeStatus("Application not found");
            return false;
        }

        await statusService.ChangeStatus("Launching  application...");
        try
        {
            ProcessStart(settingsService.Settings!.ExePath);
        }
        catch (Exception e)
        {
            logger.LogError("[{application}] Failed to launch application: {message} ", settingsService.Settings!.Parameter, e.Message);
            await statusService.ChangeStatus("Failed to launch application");
            return false;
        }
        
        logger.LogInformation("[{application}] application launched", settingsService.Settings!.Parameter);
        return true;
    }

    public virtual void ProcessStart(string path)
    {
        Process.Start(path);
    }

    public void Shutdown()
    {
        ShutdownRequested?.Invoke();
    }
}