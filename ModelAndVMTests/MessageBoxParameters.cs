using System.Windows;

namespace ModelAndVMTests;

public class MessageBoxParameters
{
    public string Text { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public MessageBoxButton Button { get; set; }
}
