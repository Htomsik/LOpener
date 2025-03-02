namespace UICore.Services.UpdateService;

public interface IUpdateService
{
    void Update();

    
    /// <summary>
    ///     Check
    /// </summary>
    bool NeedUpdate();
    
    /// <summary>
    ///     Check available update files
    /// </summary>
    bool CanUpdate();
}