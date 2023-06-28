using Avalonia.Controls;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class LogRowDetail : UserControl
    {
        public LogRowDetail()
        {
            InitializeComponent();
            DataContext = DependencyInjectionProvider.GetService<LogRowDetailViewModel>();
        }
    }
}
