using WillCDev.Components.Window;

public interface IWindowService
{
    int GetMaxWindows();
    int GetWindowCount();
    Task OpenWindow(ProgramWindow window);
    Task CloseWindow(int windowId);
    void Subscribe(Func<ProgramWindow?[], Task> callback);
}