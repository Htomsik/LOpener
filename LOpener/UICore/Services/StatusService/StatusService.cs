using System;
using System.Threading.Tasks;

namespace UICore.Services.StatusService;

public class StatusService : IStatusService
{
    public int StageDelayInMs { get; set; } = 500;
    
    public event Action<string>? Status;
    
    public async Task ChangeStatus(string status)
    {
        Status?.Invoke(status);
        await Task.Delay(StageDelayInMs);
    }
}