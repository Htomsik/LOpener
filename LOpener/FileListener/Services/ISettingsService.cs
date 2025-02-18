using FileListener.Models;

namespace FileListener.Services;

/// <summary>
///     Application settings service
/// </summary>
public interface ISettingsService
{
    /// <summary>
    ///     Application settings
    /// </summary>
    public Settings Settings { get; }
    
    /// <summary>
    ///     Read outer conf and place to local storage
    /// </summary>
    public void ReadSettings();
}