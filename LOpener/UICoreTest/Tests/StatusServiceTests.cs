using UICore.Services.StatusService;

namespace UICoreTest.Tests;

public class StatusServiceTests
{
    [Fact]
    public async Task InvokeStatusEvent()
    {
        // Arrange
        var statusService = new StatusService();
        string receivedStatus = "";
        statusService.Status += (status) => receivedStatus = status;

        // Act
        await statusService.ChangeStatus("TestStatus");

        // Assert
        Assert.Equal("TestStatus", receivedStatus);
    }

    [Fact]
    public async Task DelayByStageDelayInMs()
    {
        // Arrange
        var statusService = new StatusService
        {
            StageDelayInMs = 1000
        };
        var startTime = DateTime.UtcNow;

        // Act
        await statusService.ChangeStatus("TestStatus");
        var elapsedTime = DateTime.UtcNow - startTime;
        
        // Assert
        Assert.True(elapsedTime.TotalMilliseconds >= statusService.StageDelayInMs, $"Expected delay {statusService.StageDelayInMs}ms, got {elapsedTime.TotalMilliseconds}ms");
    }
}