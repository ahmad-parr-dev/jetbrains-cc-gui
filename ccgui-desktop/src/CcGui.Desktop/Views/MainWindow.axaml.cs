using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CcGui.Desktop.ViewModels;

namespace CcGui.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        PromptBox.KeyDown += OnPromptKeyDown;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        // Keep the message stream pinned to the newest message.
        if (DataContext is MainViewModel vm)
        {
            vm.Messages.CollectionChanged += OnMessagesChanged;
            ChatScroll.ScrollToEnd();
        }
    }

    private void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            ChatScroll.ScrollToEnd();
    }

    private void OnPromptKeyDown(object? sender, KeyEventArgs e)
    {
        // Enter sends; Shift+Enter inserts a newline.
        if (e.Key == Key.Enter && !e.KeyModifiers.HasFlag(KeyModifiers.Shift))
        {
            e.Handled = true;
            if (DataContext is MainViewModel vm && vm.SendPromptCommand.CanExecute(null))
                vm.SendPromptCommand.Execute(null);
        }
    }
}
