using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using UICore.Services.SettingsService;

namespace UICoreTest.Tests;

public class MemorySettingsServiceTests : IDisposable
{
    private readonly Mock<ILogger<MemorySettingsService>> _mockLogger = new Mock<ILogger<MemorySettingsService>>();
    private readonly Data _data = new();
    
    public MemorySettingsServiceTests()
    {
        _data.CreateTempData();
    }
    
    public void Dispose() => _data.DeleteTempData();
    
    [Fact]
    public void ConfigurationIsValid()
    {
        // Arrange
        var configuration = _data.CreateConfiguration();
        
        // Act
        var service = new MemorySettingsService(_mockLogger.Object, configuration);

        // Assert
        Assert.NotNull(service.Settings);
        Assert.Equal(_data.AppName, service.Settings.Parameter);
    }

    [Fact]
    public void CurrentAppAndDefaultAppEmpty()
    {
        // Arrange
        var configuration = _data.CreateConfiguration(string.Empty, string.Empty);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MemorySettingsService(_mockLogger.Object, configuration));
    }

    [Fact]
    public void AppIsNotImplemented()
    {
        // Arrange
        var configuration = _data.CreateConfiguration(null, null, "NoApp");

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => new MemorySettingsService(_mockLogger.Object, configuration));
    }

    [Fact]
    public void ExePathInvalid()
    {
        // Arrange
        var configuration = _data.CreateConfiguration(null, null, null, Path.Combine(_data.AppDirectory, "NoPath.exe"));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MemorySettingsService(_mockLogger.Object, configuration));
    }

    [Fact]
    public void AvailableAppsNotConfigured()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<MemorySettingsService>>();
        var inMemorySettings = new Dictionary<string, string>
        {
            { "CurrentApp", "App1" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MemorySettingsService(mockLogger.Object, configuration));
    }

}