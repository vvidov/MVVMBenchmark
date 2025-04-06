using System.Windows;
using MVVMBenchmark.ViewModels;

namespace MVVMBenchmark
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new PersonViewModel();
        }
    }
}
