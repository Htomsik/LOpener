using Avalonia;
using System;
using Serilog;

#if (RELEASE)
    using Serilog.Formatting.Json;
#endif

namespace AvaloniaUI;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        InitLogger();
        
        Log.Logger.Warning("Application started");

        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Log.Logger.Fatal("Application terminated unexpectedly. Message: {message}. Stack:{stackTrace}", e.Message, e.StackTrace);
        }
        finally
        {
            Log.Logger.Information("Application exited");
            Log.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    
    
    private static void InitLogger()
    {
                    
#if (DEBUG)
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
            .CreateLogger();
#elif (RELEASE)
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path :@"Log.json",
                        rollingInterval: RollingInterval.Infinite, 
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                        formatter: new JsonFormatter())
                .CreateLogger();
#endif

    }
}