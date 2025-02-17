using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UICore.Models;

namespace UICore.Services.SettingsService;

/// <summary>
///     Memory application settings
/// </summary>
public sealed class MemorySettingsService : ISettingsService
{
    public Settings? Settings { get; private set; }
    
    private readonly ILogger<MemorySettingsService> _logger;
    
    private readonly IConfiguration _configuration;

    public MemorySettingsService(ILogger<MemorySettingsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
        InitSettings();
    }
    
    /// <summary>
    ///     Initialize appSettings
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void InitSettings()
    {
        // Get current used APP
        var appParameter = GetCurrentApp();
        
        // Get settings for this app
        var availableSettings = GetAvailableApps();
        var setting = availableSettings.FirstOrDefault(x=>x.Parameter == appParameter);

        if (setting == null)
        {
            var availableApps = string.Join("",availableSettings.Select(x => x.Parameter).ToList());
            throw new NotImplementedException( $"App {appParameter} is not implemented. Available apps: {availableApps}");
        }
        
        _logger.LogWarning("Current application: {app}", appParameter);
        Settings = setting;
    }
    
    /// <summary>
    ///    Current app from external app configuration
    /// </summary>
    /// <exception cref="ArgumentNullException"> IF no used app configuration</exception>
    private string GetCurrentApp()
    {
        var currentApp = _configuration.GetSection("CurrentApp").Get<string>();
        if (!string.IsNullOrEmpty(currentApp))
        {
            return currentApp!;
        }
        
        _logger.LogWarning("Parameter CurrentApp is empty. Will be used default parameter");
            
        currentApp = _configuration.GetSection("DefaultApp").Get<string>();
        if (string.IsNullOrEmpty(currentApp))
        {
            throw new ArgumentNullException(nameof(currentApp),"Parameters CurrentApp and DefaultApp is empty. Can't start application");
        }

        return currentApp!;
    }

    /// <summary>
    ///     Available apps from external app configuration
    /// </summary>
    /// <exception cref="ArgumentNullException">If available apps is not configured</exception>
    private ICollection<Settings> GetAvailableApps()
    {
        var section = _configuration.GetSection("AvailableApps");
        var availableApps = section.Get<ICollection<Settings>>();
        
        if (availableApps is null || availableApps.Count == 0)
        {
            throw new ArgumentNullException(nameof(availableApps), "No available apps configured");  
        }
        
        return availableApps;
    }
}