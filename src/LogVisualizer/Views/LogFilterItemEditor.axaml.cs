using Avalonia.Controls;
using LogVisualizer.CustomControls;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class LogFilterItemEditor : UserControl
    {
        public LogFilterItemEditor()
        {
            InitializeComponent();
            DataContext = DependencyInjectionProvider.GetService<LogFilterItemEditorViewModel>();
        }
    }
}
