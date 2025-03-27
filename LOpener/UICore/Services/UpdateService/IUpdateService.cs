using System.Threading.Tasks;
using Base.Models;
using UICore.Models;

namespace UICore.Services.UpdateService;

public interface IUpdateService
{
    /// <summary>
    ///     Update files from remote to target
    /// </summary>
    Task<bool> Update();
    
    /// <summary>
    ///     Comparison target files with remote
    /// </summary>
    bool NeedUpdate();
    
    /// <summary>
    ///     Check available update files
    /// </summary>
    bool CanUpdate();

    /// <summary>
    ///     Get local update info and set it to cache
    /// </summary>
    public UpdaterInfo? GetAppUpdaterInfo();

    /// <summary>
    ///     Get remote update info and set it to cache
    /// </summary>
    public UpdaterInfo? GetRemoteUpdaterInfo();
}