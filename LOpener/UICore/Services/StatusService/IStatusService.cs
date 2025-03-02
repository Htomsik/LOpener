using System;
using System.Threading.Tasks;

namespace UICore.Services.StatusService;

public interface IStatusService
{
    /// <summary>
    ///     Delay between stages
    /// </summary>
    /// <remarks>It needs when using desktop applications for best user perfomance</remarks>
    public int StageDelayInMs { get; set; }
    
    /// <summary>
    ///     Update status
    /// </summary>
    public event Action<string> Status;
    
    /// <summary>
    ///     Invoking status
    /// </summary>
    public Task ChangeStatus(string status);

}