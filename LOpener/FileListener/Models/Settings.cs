using System.Text.Json.Serialization;

namespace FileListener.Models;

/// <summary>
///     Application settings
/// </summary>
public record Settings(string AppName, 
    string UpdateDirectoryPath, 
    string UpdateFileName, 
    string UpdateArchiveFileName,
    IEnumerable<string> FileFormats, 
    int DelayInSeconds, 
    ConfigurationType ConfigurationType = ConfigurationType.Outer)
{
    /// <summary>
    ///  Application for which data collect
    /// </summary>
    public string AppName { get; init; } = string.IsNullOrEmpty(AppName) ? "DEF" : AppName;
    
    /// <summary>
    ///     Directory with files needed to collect for update.
    ///     Also used for save update info file
    /// </summary>
    public string UpdateDirectoryPath { get; } = string.IsNullOrEmpty(UpdateDirectoryPath) ? AppContext.BaseDirectory : UpdateDirectoryPath;
    
    /// <summary>
    ///     Update Info file name without path
    /// </summary>
    /// <example> Update.JSON </example>
    public string UpdateFileName { get; } = UpdateFileName;
    
    public string UpdateFilePath => Path.Combine(UpdateDirectoryPath, UpdateFileName);

    /// <summary>
    ///     Compressed file name without path
    /// </summary>
    /// <example> Update.zip </example>
    public string UpdateArchiveFileName { get; } = UpdateArchiveFileName;
    
    public string UpdateArchiveFilePath => Path.Combine(UpdateDirectoryPath, UpdateArchiveFileName);
    
    /// <summary>
    ///     Collected file formats
    /// </summary>
    /// <example> DLL, EXE, JSON or something else</example>
    public IEnumerable<string> FileFormats { get; } = FileFormats;
    
    /// <summary>
    ///     Collect info delay
    /// </summary>
    public int DelayInSeconds { get; } = DelayInSeconds;

    /// <summary>
    ///     Configuration type for best testing
    /// </summary>
    [JsonIgnore]
    public ConfigurationType ConfigurationType { get; init; } = ConfigurationType;

}

public enum ConfigurationType : byte
{
    Default,
    Outer
}