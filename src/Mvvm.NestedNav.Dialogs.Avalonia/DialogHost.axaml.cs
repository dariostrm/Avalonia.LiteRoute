using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using Mvvm.NestedNav.Exceptions;

namespace Mvvm.NestedNav.Dialogs.Avalonia;

public class DialogHost : TemplatedControl, IDialogHost
{
    // Null if no dialog is shown
    private INavigator? _navigator;
    private CompositeDisposable _navigatorSubscriptions = new CompositeDisposable();
    
    private bool _isDialogOpen;

    public static readonly DirectProperty<DialogHost, bool> IsDialogOpenProperty = AvaloniaProperty.RegisterDirect<DialogHost, bool>(
        nameof(IsDialogOpen), o => o.IsDialogOpen, (o, v) => o.IsDialogOpen = v);

    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set => SetAndRaise(IsDialogOpenProperty, ref _isDialogOpen, value);
    }

    private IDialogViewModel? _currentDialog;

    public static readonly DirectProperty<DialogHost, IDialogViewModel?> CurrentDialogProperty 
        = AvaloniaProperty.RegisterDirect<DialogHost, IDialogViewModel?>
        (
            nameof(CurrentDialog), 
            o => o.CurrentDialog, 
            (o, v) => o.CurrentDialog = v
        );

    public IDialogViewModel? CurrentDialog
    {
        get => _currentDialog;
        set => SetAndRaise(CurrentDialogProperty, ref _currentDialog, value);
    }
    
    public void ShowDialog(DialogScreen dialogScreen, Action onClosed)
    {
        if (_navigator is null)
        {
            _navigator = new Navigator(dialogScreen, ViewModelFactory.Instance);
            _navigator.CurrentViewModel
                .Subscribe(OnDialogLoaded)
                .DisposeWith(_navigatorSubscriptions);

            void OnDialogLoaded(IScreenViewModel vm)
            {
                if (vm is not IDialogViewModel dialogVm)
                    throw new InvalidScreenException(nameof(DialogHost));
                dialogVm.Closed += OnLastDialogClosed;
                dialogVm.Closed += (s, e) => onClosed();
                CurrentDialog = dialogVm;
                IsDialogOpen = true;
            }
        }
        else
        {
            _navigator.NavigatingBack.Subscribe(OnNavigatingBack);
            _navigator.Navigate(dialogScreen);
            
            void OnNavigatingBack(NavigatingBackEventArgs args)
            {
                if (args.RemovingScreen != dialogScreen)
                    return;
                
            }
        }
    }

    private void OnLastDialogClosed(object? sender, EventArgs e)
    {
        IsDialogOpen = false;
        _navigator = null;
        _navigatorSubscriptions.Clear();
        CurrentDialog = null;
    }

    public async Task ShowDialogAsync(DialogScreen dialogScreen)
    {
        var tcs = new TaskCompletionSource();
        
        ShowDialog(dialogScreen, () => tcs.SetResult());
        
        await tcs.Task;
    }
    
    public void ShowDialog<TDialogResult>(DialogScreen<TDialogResult> dialogScreen, Action<TDialogResult?> onClosed) 
        where TDialogResult : class
    {
        throw new NotImplementedException();
    }

    public async Task<TDialogResult?> ShowDialogAsync<TDialogResult>(DialogScreen<TDialogResult> dialogScreen) 
        where TDialogResult : class
    {
        var tcs = new TaskCompletionSource<TDialogResult?>();
        
        ShowDialog(dialogScreen, result => tcs.SetResult(result));
        
        return await tcs.Task;
    }
    
}