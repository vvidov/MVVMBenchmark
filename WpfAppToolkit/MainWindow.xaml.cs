using System.Windows;
using ViewModels;

namespace WpfAppToolkit;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new PersonViewModel2();
    }
}
