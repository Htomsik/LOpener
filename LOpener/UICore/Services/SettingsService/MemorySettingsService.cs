using UICore.Models;

namespace UICore.Services.SettingsService;

/// <summary>
///     Memory application settings
/// </summary>
public sealed class MemorySettingsService() : ISettingsService
{
    public Settings? Settings { get; private set; } 
    
    public void SetSettings(Settings settings)
    {
        Settings = settings;
    }
}