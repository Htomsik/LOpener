using AvaloniaUI.Views;
using Microsoft.Extensions.DependencyInjection;
using UICore.Services.SettingsService;
using UICore.ViewModels;

namespace AvaloniaUI.IOC;

/// <summary>
///     Initialize APP infrastructure
/// </summary>
public static class IocRegistration
{
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services
            .AddSingleton<ISettingsService, MemorySettingsService>();
    
    public static IServiceCollection ViewModelRegistration(this IServiceCollection services) =>
        services
            .AddTransient<MainWindowViewModel>();
    
    public static IServiceCollection WindowRegistration(this IServiceCollection services) =>
        services
            .AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
}