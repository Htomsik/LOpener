using System;
using AvaloniaUI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UICore.Services.SettingsService;
using UICore.Services.StatusService;
using UICore.Services.UpdateService;
using UICore.ViewModels;

namespace AvaloniaUI.IOC;

/// <summary>
///     Initialize APP infrastructure
/// </summary>
public static class IocRegistration
{
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services
            .AddSingleton<ISettingsService, MemorySettingsService>()
            .AddSingleton<IStatusService, StatusService>()
            .AddTransient<IUpdateService, DirectoryUpdateService>();
    
    public static IServiceCollection ViewModelRegistration(this IServiceCollection services) =>
        services
            .AddTransient<MainWindowViewModel>();
    
    public static IServiceCollection WindowRegistration(this IServiceCollection services) =>
        services
            .AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
    
    public static IServiceCollection ConfigurationRegistration(this IServiceCollection services, string[]? args)
    {
#if DEBUG
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.Development.json");
#elif RELEASE
         var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json");
#endif
        
        if (args is { Length: > 0 })
        {
            configurationBuilder
                .AddCommandLine(args);
        }
        else
        {
            Log.Warning("Application starting without command line arguments.");
        }
        
        var configuration = configurationBuilder.Build();
        
        services.AddSingleton<IConfiguration>(configuration);

        return services;
    }
}