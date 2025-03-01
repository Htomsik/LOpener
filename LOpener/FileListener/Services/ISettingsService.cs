using FileListener.Models;

namespace FileListener.Services;

/// <summary>
///     Application settings service
/// </summary>
public interface ISettingsService
{
    /// <summary>
    ///     Cached outer configuration
    /// </summary>
    public Settings Settings { get; }
    
    /// <summary>
    ///     Read outer configuration
    /// </summary>
    /// <remarks> If outer configuration is broken return defaultConfig</remarks>
    public Settings ReadSettings();
}