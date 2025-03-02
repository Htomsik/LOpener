using System.Threading.Tasks;

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
}