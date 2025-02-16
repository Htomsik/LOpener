namespace UICore.Models;



/// <summary>
///     Application settings   
/// </summary>
public sealed record Settings(string Title, string ExePath,  SyncSettings Sync)
{
    public string Title { get; } = Title;
    
    public string ExePath {get;} = ExePath;
    
    public SyncSettings Sync {get;} = Sync;
    
}

/// <summary>
///     File synchronizationType
/// </summary>
public sealed record SyncSettings(SyncSettingsType SettingsType, string Path)
{
    public SyncSettingsType SettingsType { get; } = SettingsType;
    
    public string Path { get; } = Path;
}

/// <summary>
///  Files Synchronization type
/// </summary>
public enum SyncSettingsType
{
    Http,
    Folder
}
