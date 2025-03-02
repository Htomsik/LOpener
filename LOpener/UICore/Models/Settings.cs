using System.IO;

namespace UICore.Models;



/// <summary>
///     Application settings   
/// </summary>
public sealed record Settings(string Title, 
    string Parameter, 
    string ExePath, 
    string DirectoryPath, 
    int UpdateDelaySeconds,
    SyncSettings Sync)
{
    public string Parameter { get; } = Parameter;
    
    public string Title { get; } = Title;
    
    public string ExePath { get; } = ExePath;
    
    public string DirectoryPath { get; } = DirectoryPath;

    public int UpdateDelaySeconds { get; } = UpdateDelaySeconds;
    
    public string UpdateFilePath => Path.Combine(DirectoryPath, Sync.UpdateFileName);
    
    public SyncSettings Sync {get;} = Sync;
}

/// <summary>
///     File synchronizationType
/// </summary>
public sealed record SyncSettings(SyncSettingsType Type, string Path, string UpdateFileName, string UpdateArchiveFileName)
{
    public SyncSettingsType Type { get; } = Type;
    
    /// <summary>
    ///     Directory or url
    /// </summary>
    /// <remarks> Directory -> directory path. Http -> url</remarks>
    public string Path { get; } = Path;
    
    /// <summary>
    ///     Update Info file name without path
    /// </summary>
    /// <example> Update.JSON </example>
    public string UpdateFileName { get; } = UpdateFileName;
    
    public string UpdateFilePath => System.IO.Path.Combine(Path, UpdateArchiveFileName);
    
    /// <summary>
    ///     Compressed file name without path
    /// </summary>
    /// <example> Update.zip </example>
    public string UpdateArchiveFileName { get; } = UpdateArchiveFileName;
    public string UpdateArchiveFilePath => System.IO.Path.Combine(Path, UpdateArchiveFileName);
    
}

/// <summary>
///  Files Synchronization type
/// </summary>
public enum SyncSettingsType
{
    Http,
    Directory
}
