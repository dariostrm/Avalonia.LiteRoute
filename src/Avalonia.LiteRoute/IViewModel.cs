namespace Avalonia.LiteRoute;

public interface IViewModel
{
    void OnBecomeVisible();
    void OnMoveToBackground();
    void OnDestroy();
}