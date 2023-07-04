using Avalonia.Controls;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class LanguageSelector : UserControl
    {
        public LanguageSelector()
        {
            InitializeComponent();
            DataContext = DependencyInjectionProvider.GetService<LanguageSelectorViewModel>();
        }
    }
}
