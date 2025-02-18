using System.Threading.Tasks;
using UICore.Services.SettingsService;

namespace UICore.Services.UpdaterService;

/// <summary>
///     Update application by file path
/// </summary>
public sealed class FileUpdaterService(ISettingsService settingsService) : IUpdater
{
    private ISettingsService SettingsService { get; } = settingsService;
    
    public Task<bool> CheckForUpdatesAsync()
    {
        throw new System.NotImplementedException();
    }
}