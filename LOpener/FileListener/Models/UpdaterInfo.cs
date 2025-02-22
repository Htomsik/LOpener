using System.Text.Json.Serialization;

namespace FileListener.Models;



/// <summary>
///     Update
/// </summary>
public record UpdaterInfo(string AppName, ICollection<FileParameter> FileParameters)
{
    /// <summary>
    ///      Application for which data collect
    /// </summary>
    public string AppName { get; set; } = AppName;

    /// <summary>
    ///     Parameters of updatedFiles
    /// </summary>
    public ICollection<FileParameter> FileParameters { get; init; } = FileParameters;
}


/// <summary>
///     Parameters of  files
/// </summary>
public record FileParameter(string FileName, 
                            DateTime LastWriteTimeUtc, 
                            [property: JsonIgnore] string FilePath)
{
}