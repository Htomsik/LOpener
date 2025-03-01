using FileListener.Services;

namespace FileListener.Workers;


/// <summary>
///     Base realization for hide logging of Workers
/// </summary>
public abstract class BaseWorker : BackgroundService
{
    protected readonly ILogger<FileWorker> Logger;
    
    protected readonly ISettingsService SettingsService;

    protected BaseWorker(ILogger<FileWorker> logger, ISettingsService settingsService )
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        SettingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    }
    
    protected abstract Task ProcessAsync(CancellationToken stoppingToken);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            Logger.LogInformation("{workerName} running at: {time} with {delay} s delay", nameof(FileWorker), DateTimeOffset.Now, SettingsService.Settings.DelayInSeconds);
            await ProcessAsync(stoppingToken);
            Logger.LogInformation("{workerName} finished at: {time}", nameof(FileWorker), DateTimeOffset.Now);
            
            await Task.Delay(SettingsService.Settings.DelayInSeconds * 1000, stoppingToken);
        }
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("{workerName} starting.", GetType().Name);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("{workerName} stopping.", GetType().Name);
        return base.StopAsync(cancellationToken);
    }
}