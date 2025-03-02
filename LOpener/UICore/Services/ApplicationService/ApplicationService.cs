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
    
    public async Task LaunchTarget()
    {
        if (!File.Exists(settingsService.Settings?.ExePath))
        {
            logger.LogError("[{application}] exe file not found: {exe path}", settingsService.Settings!.Parameter, settingsService.Settings!.ExePath);
            await statusService.ChangeStatus("Application not found");
            return;
        }

        await statusService.ChangeStatus("Launching  application...");
        try
        {
            Process.Start(settingsService.Settings!.ExePath);
        }
        catch (Exception e)
        {
            logger.LogError("[{application}] Failed to launch application: {message} ", settingsService.Settings!.Parameter, e.Message);
            await statusService.ChangeStatus("Failed to launch application");
            return;
        }
        
        logger.LogInformation("[{application}] application launched", settingsService.Settings!.Parameter);
    }

    public void Shutdown()
    {
        ShutdownRequested?.Invoke();
    }
}