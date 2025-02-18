namespace FileListener.Models;

/// <summary>
///     Application settings
/// </summary>
public record Settings(string UpdateDirectoryPath, string UpdateFileName, IEnumerable<string> FileFormats, int DelayInSeconds)
{
    /// <summary>
    ///     Directory with files needed to collect for update
    /// </summary>
    public string UpdateDirectoryPath { get; } = UpdateDirectoryPath;
    
    /// <summary>
    ///     Name wothout path of file for update info
    /// </summary>
    /// <example> Update.JSON </example>
    public string UpdateFileName { get; } = UpdateFileName;
    
    /// <summary>
    ///     Service update format
    /// </summary>
    public IEnumerable<string> FileFormats { get; } = FileFormats;
    
    /// <summary>
    ///     Update files delay
    /// </summary>
    public int DelayInSeconds { get; } = DelayInSeconds;
};