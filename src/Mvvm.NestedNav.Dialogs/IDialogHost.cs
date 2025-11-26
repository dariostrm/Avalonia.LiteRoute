namespace Mvvm.NestedNav.Dialogs;

public interface IDialogHost
{
    bool IsDialogOpen { get; }
    
    void ShowDialog(DialogScreen dialogScreen, Action onClosed);
    
    Task ShowDialogAsync(DialogScreen dialogScreen);
    
    void ShowDialog<TDialogResult>(
        DialogScreen<TDialogResult> dialogScreen,
        Action<TDialogResult?> onClosed
    ) where TDialogResult : class;
    
    Task<TDialogResult?> ShowDialogAsync<TDialogResult>(DialogScreen<TDialogResult> dialogScreen) 
        where TDialogResult : class;
}