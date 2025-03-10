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
using UICore.Services.ApplicationService;

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
                    .ConfigurationRegistration(desktop.Args)
                    .ServiceRegistration()
                    .ViewModelRegistration()
                    .WindowRegistration()
                    .BuildServiceProvider());
            
            desktop.MainWindow = Ioc.Default.GetService<MainWindow>();
            
            Ioc.Default.GetService<IApplicationService>()!.ShutdownRequested += () =>
            {
                desktop.Shutdown();
            };
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