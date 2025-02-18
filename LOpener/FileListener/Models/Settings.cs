namespace FileListener.Models;

/// <summary>
///     Application settings
/// </summary>
public record Settings(IEnumerable<string> FileFormats, int DelayInSeconds )
{

    /// <summary>
    ///     Service update format
    /// </summary>
    public IEnumerable<string> FileFormats { get; } = FileFormats;
    
    /// <summary>
    ///     Update files delay
    /// </summary>
    public int DelayInSeconds { get; } = DelayInSeconds;
};