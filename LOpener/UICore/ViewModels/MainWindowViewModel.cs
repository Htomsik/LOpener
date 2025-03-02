
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UICore.Services.ApplicationService;
using UICore.Services.SettingsService;
using UICore.Services.StatusService;
using UICore.Services.UpdateService;


namespace UICore.ViewModels;


public partial  class MainWindowViewModel : ViewModelBase
{
    private readonly IUpdateService _updateService;
    
 
    
    private readonly IStatusService _statusService;

    [ObservableProperty] private string? _title = "LOpener";

    [ObservableProperty] private string? _status = "Wait...";
    
    public  MainWindowViewModel(ISettingsService settingsService, 
        IUpdateService updateService, 
        IStatusService statusService)
    {
        _updateService = updateService;
        _statusService = statusService;
        _title = settingsService.Settings?.Title;
        
        statusService.Status += (newStatus)=>{ Status = newStatus; };

        Update();
    }

#if DEBUG
    /// <summary>
    ///     Only for xaml preview
    /// </summary>
    public MainWindowViewModel()
    {
    }
#endif
    
    [RelayCommand]
    public async void Update()
    {
        await _updateService.Update();
    }
}