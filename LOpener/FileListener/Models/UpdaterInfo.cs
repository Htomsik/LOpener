using System.Text.Json.Serialization;

namespace FileListener.Models;



public record UpdaterInfo(string AppName, ICollection<FileParameter> FileParameters) : Base.Models.UpdaterInfo<FileParameter>(AppName, FileParameters)
{
}


public record FileParameter(string FileName, DateTime LastWriteTime, string FilePath) : Base.Models.FileParameter(FileName,LastWriteTime)
{
    [property: JsonIgnore] public string FilePath { get; } = FilePath;
}