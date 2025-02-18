namespace FileListener.Models;

/// <summary>
///     Application settings
/// </summary>
public record Settings(string AppName, string UpdateDirectoryPath, string UpdateFileName, IEnumerable<string> FileFormats, int DelayInSeconds)
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
    ///     Name without path
    /// </summary>
    /// <example> Update.JSON </example>
    public string UpdateFileName { get; } = UpdateFileName;
    
    /// <summary>
    ///     Collected file formats
    /// </summary>
    /// <example> DLL, EXE, JSON or something else</example>
    public IEnumerable<string> FileFormats { get; } = FileFormats;
    
    /// <summary>
    ///     Collect info delay
    /// </summary>
    public int DelayInSeconds { get; } = DelayInSeconds;
};