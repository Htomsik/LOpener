using Microsoft.Extensions.Logging;
using Moq;
using UICore.Services.ApplicationService;
using UICore.Services.SettingsService;
using UICore.Services.StatusService;

namespace UICoreTest.Tests;

public class ApplicationServiceTests : IDisposable
{
    private readonly Mock<ILogger<ApplicationService>> _mockLogger;
    private readonly Mock<ISettingsService> _mockSettingsService;
    private readonly Mock<IStatusService> _mockStatusService;
    private readonly Data _data = new();

    public ApplicationServiceTests()
    {
        _mockLogger = new Mock<ILogger<ApplicationService>>();
        _mockSettingsService = new Mock<ISettingsService>();
        _mockStatusService = new Mock<IStatusService>();
        
        _data.CreateTempData();
    }

    public void Dispose() => _data.DeleteTempData();
    
    
    [Fact]
    public async Task ExePathDoesNotExist()
    {
        // Arrange
        var settings = _data.CreateSettings(null,"NoPath.exe");
        _mockSettingsService.Setup(x => x.Settings).Returns(settings);

        var service = new ApplicationService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);

        // Act
        var result = await service.LaunchTarget();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExePathExists()
    {
        // Arrange
        var settings = _data.CreateSettings();
        _mockSettingsService.Setup(x => x.Settings).Returns(settings);

        var mockService = new Mock<ApplicationService>(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object)
        {
            CallBase = true
        };

        mockService.Setup(x => x.ProcessStart(It.IsAny<string>())).Verifiable();
        var service = mockService.Object;

        // Act
        var result = await service.LaunchTarget();

        // Assert
        Assert.True(result);
        mockService.Verify(x => x.ProcessStart(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ProcessStartFails()
    {
        // Arrange
        var settings = _data.CreateSettings();
        _mockSettingsService.Setup(x => x.Settings).Returns(settings);

        var mockService = new Mock<ApplicationService>(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object)
        {
            CallBase = true
        };

        mockService.Setup(x => x.ProcessStart(It.IsAny<string>())).Throws(new Exception("Process start failed"));
        var service = mockService.Object;

        // Act
        var result = await service.LaunchTarget();

        // Assert
        Assert.False(result);
        mockService.Verify(x => x.ProcessStart(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void ShutdownRequestedEvent()
    {
        // Arrange
        var service = new ApplicationService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);

        var shutdownRequest = false;
        service.ShutdownRequested += () => shutdownRequest = true;

        // Act
        service.Shutdown();

        // Assert
        Assert.True(shutdownRequest);
    }
}