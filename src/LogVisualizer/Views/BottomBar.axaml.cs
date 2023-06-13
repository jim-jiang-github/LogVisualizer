using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class BottomBar : UserControl
    {
        public BottomBar()
        {
            InitializeComponent();
            //WeakReferenceMessenger.Default.Register<ScenarioConfigViewModel>(this, (r, m) =>
            //{
            //    Log.Information("BottomBar receive scenarioConfig.Flyout:{f} ScenarioConfigViewModel.Enabled:{e}", scenarioConfig.Flyout, m.Enabled);
            //    if (scenarioConfig.Flyout is { } && !m.Enabled)
            //    {
            //        scenarioConfig.Flyout.Hide();
            //    }
            //});
        }
    }
}
