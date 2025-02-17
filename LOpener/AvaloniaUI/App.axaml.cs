using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using AvaloniaUI.IOC;
using AvaloniaUI.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UICore.Models;
using UICore.Services.SettingsService;

namespace AvaloniaUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // DI Configuration
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddLogging(builder => builder.AddSerilog())
                    .ServiceRegistration()
                    .ViewModelRegistration()
                    .WindowRegistration()
                    .BuildServiceProvider());
            
            // TODO Change to appsettings.json
            var memorySettings = new Settings("APP", "",new SyncSettings(SyncSettingsType.Folder, ""));
            Ioc.Default.GetService<ISettingsService>()?.SetSettings(memorySettings);
            
            desktop.MainWindow = Ioc.Default.GetService<MainWindow>();
        }
        
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}