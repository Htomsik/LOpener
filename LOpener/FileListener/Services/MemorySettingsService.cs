using FileListener.Extension;
using FileListener.Models;

namespace FileListener.Services;

/// <summary>
///     In Memory from App configuration settings service
/// </summary>
public class MemorySettingsService(ILogger<MemorySettingsService> logger, IConfiguration configuration)
    : ISettingsService
{
    public Settings Settings => _settings ??= ReadSettings();
    private Settings? _settings;

    public Settings ReadSettings()
    {
        var settings =  
            new Settings("NoAPP",
                Directory.GetCurrentDirectory(),
                "Update.JSON", 
                new []{"DLL"},
                3600,
                ConfigurationType.Default);

        var section = configuration.GetSection(AppSettingsConst.FileUpdatePolitics);
        if (!section.Exists())
        {
            logger.LogWarning("{sectionName}  section not found. Will be used default parameters", AppSettingsConst.FileUpdatePolitics);
            return settings;
        }
        
        try
        {
           var sectionSettings = section.Get<Settings>();
           if (sectionSettings != null)
           {
               settings = sectionSettings;
           }
           else
           {
               logger.LogWarning("{sectionName} section can't implement to class {settingsClassName}. Will be used default parameters", AppSettingsConst.FileUpdatePolitics, nameof(Settings));
           }
        }
        catch
        {
            logger.LogWarning("{sectionName} section can't implement to class {settingsClassName}. Will be used default parameters", AppSettingsConst.FileUpdatePolitics, nameof(Settings)); 
        }

        return settings;
    }
}