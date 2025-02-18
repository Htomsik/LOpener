using System.Threading.Tasks;

namespace UICore.Services.UpdaterService;

/// <summary>
///     DLL updater
/// </summary>
public interface IUpdater
{
     Task<bool> CheckForUpdatesAsync();
}