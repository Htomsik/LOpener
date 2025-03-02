using System;
using System.Collections.Generic;

namespace UICore.Models;

public sealed record UpdaterInfo(string AppName, ICollection<FileParameter> FileParameters) : Base.Models.UpdaterInfo<FileParameter>(AppName, FileParameters)
{
}


public sealed record FileParameter(string FileName, DateTime LastWriteTime) : Base.Models.FileParameter(FileName,LastWriteTime)
{
   
}