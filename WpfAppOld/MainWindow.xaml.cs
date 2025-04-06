using System.Windows;
using ViewModels;

namespace WpfAppOld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new PersonViewModelOldStyle();
        }
    }
}