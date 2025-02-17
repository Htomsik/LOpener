using System;
using AvaloniaUI.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
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
    
    public static IServiceCollection ConfigurationRegistration(this IServiceCollection services, string[]? args)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appSettings.json");
        
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