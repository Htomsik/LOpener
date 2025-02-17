namespace UICore.Models;



/// <summary>
///     Application settings   
/// </summary>
public sealed record Settings(string Title, string Parameter, string ExePath,  SyncSettings Sync)
{
    public string Parameter { get; } = Parameter;
    public string Title { get; } = Title;
    
    public string ExePath {get;} = ExePath;
    
    public SyncSettings Sync {get;} = Sync;
    
}

/// <summary>
///     File synchronizationType
/// </summary>
public sealed record SyncSettings(SyncSettingsType Type, string Path)
{
    public SyncSettingsType Type { get; } = Type;
    
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
