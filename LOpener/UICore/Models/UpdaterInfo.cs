using System;
using System.Collections.Generic;
using System.Linq;

namespace UICore.Models;

public sealed record UpdaterInfo(string AppName, 
    ICollection<FileParameter> FileParameters, 
    DateTime LastUpdateTime 
    ) : Base.Models.UpdaterInfo<FileParameter>(AppName, FileParameters)
{
    /// <summary>
    ///     Last sync time
    /// </summary>
    public DateTime LastUpdateTime { get; } = LastUpdateTime;


    public bool Equals(UpdaterInfo? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return AppName == other.AppName &&
               FileParameters.SequenceEqual(other.FileParameters);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ LastUpdateTime.GetHashCode();
        }
    }
}


public sealed record FileParameter(string FileName, DateTime LastWriteTime) : Base.Models.FileParameter(FileName,LastWriteTime)
{
   
}