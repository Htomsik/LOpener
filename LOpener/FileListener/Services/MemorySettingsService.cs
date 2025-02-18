using FileListener.Extension;
using FileListener.Models;

namespace FileListener.Services;

/// <summary>
///     In Memory from App configuration settings service
/// </summary>
public class MemorySettingsService : ISettingsService
{
    public Settings Settings { get; private set; }
    
    private readonly ILogger<MemorySettingsService> _logger;
    
    private readonly IConfiguration _configuration;

    public MemorySettingsService(ILogger<MemorySettingsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        Settings = new Settings(["DLL"], 10000);
        
        ReadSettings();
    }
    
    public void ReadSettings()
    {
        var section = _configuration.GetSection(AppSettingsConst.FileUpdatePolitics);
        if (!section.Exists())
        {
            _logger.LogWarning("{sectionName}  section not found. Will be used default parameters", AppSettingsConst.FileUpdatePolitics);
            return;
        }
        
        try
        {
           var sectionSettings = section.Get<Settings>();
           if (sectionSettings != null)
           {
               Settings = sectionSettings;
           }
        }
        catch
        {
            _logger.LogWarning("{sectionName} section can't implement to class {settingsClassName}. Will be used default parameters", AppSettingsConst.FileUpdatePolitics, nameof(Settings)); 
        }
    }
}