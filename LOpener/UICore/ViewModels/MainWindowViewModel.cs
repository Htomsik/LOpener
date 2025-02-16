
using CommunityToolkit.Mvvm.ComponentModel;

using UICore.Services.SettingsService;


namespace UICore.ViewModels;


public partial  class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string? _title;
    
    public MainWindowViewModel(ISettingsService settingsService)
    {
        _title = settingsService.Settings?.Title;
    }
}