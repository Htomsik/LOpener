using System.Collections.Concurrent;
using FileListener.Models;
using FileListener.Services;

namespace FileListener.Workers;

public sealed class FileWorker : BaseWorker
{
    public FileWorker(ILogger<FileWorker> logger, ISettingsService settingsService) : base(logger, settingsService)
    {
        
    }
    
    /// <summary>
    ///     Process one operation unit (exclude logging) 
    /// </summary>
    protected override async Task ProcessAsync(CancellationToken stoppingToken)
    {
        
    }
}