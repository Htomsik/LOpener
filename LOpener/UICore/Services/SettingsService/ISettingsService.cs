using UICore.Models;

namespace UICore.Services.SettingsService;

/// <summary>
///     Application settings manager
/// </summary>
public interface ISettingsService
{
    public Settings? Settings { get; }
    
}