using System.Windows;

namespace Services;

public interface IMessageBoxService
{
    MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
}

public class DefaultMessageBoxService : IMessageBoxService
{
    public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        return MessageBox.Show(messageBoxText, caption, button, icon);
    }
}

public class TestMessageBoxService : IMessageBoxService
{
    private readonly Func<string, string, MessageBoxButton, MessageBoxImage, MessageBoxResult> _showDelegate;

    public TestMessageBoxService(Func<string, string, MessageBoxButton, MessageBoxImage, MessageBoxResult> showDelegate)
    {
        _showDelegate = showDelegate;
    }

    public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        return _showDelegate(messageBoxText, caption, button, icon);
    }
}
