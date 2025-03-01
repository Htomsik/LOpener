using FileListener;
using FileListener.Services;
using FileListener.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ISettingsService, MemorySettingsService>();
builder.Services.AddHostedService<FileWorker>();

var host = builder.Build();

host.Run();