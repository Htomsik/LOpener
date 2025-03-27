using Microsoft.Extensions.Logging;
using Moq;
using UICore.Models;
using UICore.Services.SettingsService;
using UICore.Services.StatusService;
using UICore.Services.UpdateService;

namespace UICoreTest.Tests;

public class DirectoryUpdateServiceTests : IDisposable
{
    private readonly Mock<ILogger<DirectoryUpdateService>> _mockLogger;
    private readonly Mock<ISettingsService> _mockSettingsService;
    private readonly Mock<IStatusService> _mockStatusService;
    private readonly Data _data = new();
    
    public DirectoryUpdateServiceTests()
    {
        _mockLogger = new Mock<ILogger<DirectoryUpdateService>>();
        _mockSettingsService = new Mock<ISettingsService>();
        _mockStatusService = new Mock<IStatusService>();
        
        _data.CreateTempData();
        
        var settings = new Settings(
            _data.AppName,
            _data.AppName,
            _data.AppExePath,
            _data.AppDirectory,
            10,
            new SyncSettings(SyncSettingsType.Directory, _data.RemoteDirectory, "update.json", "update.zip")
        );

        _mockSettingsService.Setup(x => x.Settings).Returns(settings);
    }

    public void Dispose() => _data.DeleteTempData();

    [Theory]
    [InlineData(false, true, true, true, true, false)] // Remote directory doesn't exist
    [InlineData(true, false, true, true, true, false)] // Remote update file doesn't exist
    [InlineData(true, true, false, true, true, false)] // Remote update archive doesn't exist
    [InlineData(true, true, true, false, true, false)] // App directory doesn't exist
    [InlineData(true, true, true, true, false, false)] // App update file doesn't exist
    [InlineData(true, true, true, true, true, true)]   // All files exists
    public void CanUpdateFilesExpectedResult(
        bool remoteDirectoryExists,
        bool remoteUpdateFileExists,
        bool remoteUpdateArchiveExists,
        bool appDirectoryExists,
        bool appUpdateFileExists,
        bool expectedResult)
    {
        _data.CreateTempData();
        
        if (!remoteDirectoryExists) Directory.Delete(_data.RemoteDirectory, true);
        if (!remoteUpdateFileExists) File.Delete(_data.RemoteUpdateFilePath);
        if (!remoteUpdateArchiveExists) File.Delete(_data.RemoteUpdateArchiveFilePath);
        if (!appDirectoryExists) Directory.Delete(_data.AppDirectory, true);
        if (!appUpdateFileExists) File.Delete(_data.AppUpdateFilePath);
        
        var service = new DirectoryUpdateService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);

        if (remoteUpdateFileExists)
            return;
        
        // Act
        var result = service.CanUpdate();

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(20, true)]  
    [InlineData(5, false)] 
    public void NeedUpdateDelayExpectedResult(
        int secondsElapsed, 
        bool expectedResult)
    {
        // Arrange
        var remoteUpdaterInfo = new UpdaterInfo(
            _data.AppName, 
            new List<FileParameter>()
            {
                new FileParameter("app1.dll", DateTime.Now),
            }, 
            DateTime.Now
        );
        var appUpdaterInfo = new UpdaterInfo(
            _data.AppName, 
            new List<FileParameter>(), 
            remoteUpdaterInfo.LastUpdateTime.AddSeconds(-secondsElapsed)
        );
        
        _data.CreateTempData(appUpdaterInfo, remoteUpdaterInfo);
        
        var service = new DirectoryUpdateService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);
    
        // Act
        var result = service.NeedUpdate();
    
        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetRemoteUpdaterInfoExpectedResult(bool isValidJson)
    {
        // Arrange
        var expectedUpdaterInfo = isValidJson 
            ? new UpdaterInfo(
                _data.AppName,
                new List<FileParameter> 
                { 
                    new FileParameter("app1.dll", DateTime.Now),
                    new FileParameter("app2.dll", DateTime.Now) 
                },
                DateTime.Now
            ) 
            : null;
        
        _data.CreateTempData(null, expectedUpdaterInfo);
        var service = new DirectoryUpdateService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);
        
        // Act
        var result = service.GetRemoteUpdaterInfo();

        // Assert
        if (isValidJson)
        {
            Assert.NotNull(result);
            Assert.True(result.Equals(expectedUpdaterInfo));
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetAppUpdaterInfoExpectedResult(bool isValidJson)
    {
        // Arrange
        var expectedUpdaterInfo = isValidJson 
            ? new UpdaterInfo(
                _data.AppName,
                new List<FileParameter> 
                { 
                    new FileParameter("app1.dll", DateTime.Now),
                    new FileParameter("app2.dll", DateTime.Now.AddDays(-1)) 
                },
                DateTime.Now
            ) 
            : null;

        // Подготовка данных
        _data.CreateTempData(expectedUpdaterInfo, null);
        var service = new DirectoryUpdateService(_mockLogger.Object, _mockSettingsService.Object, _mockStatusService.Object);

        // Act
        var result = service.GetAppUpdaterInfo();

        // Assert
        if (isValidJson)
        {
            Assert.NotNull(result);
            Assert.True(result.Equals(expectedUpdaterInfo));
        }
        else
        {
            Assert.Null(result);
        }
    }

}