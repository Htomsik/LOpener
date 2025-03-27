using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Base.Models;


public record UpdaterInfo<T>(string AppName, ICollection<T> FileParameters) where T: FileParameter
{
    /// <summary>
    ///      Application for which data collect
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string AppName { get; set; } = AppName;

    /// <summary>
    ///     Parameters of updatedFiles
    /// </summary>
    public ICollection<T> FileParameters { get; } = FileParameters;
}


/// <summary>
///     Parameters of  files
/// </summary>
public record FileParameter(string FileName, DateTime LastWriteTime)
{
    public string FileName { get; set; } = FileName;
    
    public DateTime LastWriteTime { get; set; } = LastWriteTime;
}
