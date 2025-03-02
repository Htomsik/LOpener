using System;
using System.Threading.Tasks;

namespace UICore.Services.ApplicationService;

public interface IApplicationService
{
    /// <summary>
    ///     Informer about shutdown
    /// </summary>
    public event Action ShutdownRequested;
    
    /// <summary>
    ///     Launch target application
    /// </summary>
    public Task LaunchTarget();
    
    /// <summary>
    ///     Shutdown updater
    /// </summary>
    public void Shutdown();
}