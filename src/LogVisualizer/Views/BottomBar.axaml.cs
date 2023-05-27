using Avalonia.Controls;
using LogVisualizer.ViewModels;

namespace LogVisualizer.Views
{
    public partial class BottomBar : UserControl
    {
        public BottomBar()
        {
            InitializeComponent();
        }

        //protected override void OnLoaded()
        //{
        //    base.OnLoaded();
        //    var schemaConfigViewModel = DependencyInjectionProvider.GetService<SchemaConfigViewModel>();
        //    if (schemaConfigViewModel == null)
        //    {
        //        return;
        //    }
        //    schemaConfigViewModel.PropertyChanged += SchemaConfigViewModel_PropertyChanged;
        //}

        //protected override void OnUnloaded()
        //{
        //    base.OnUnloaded();
        //    var schemaConfigViewModel = DependencyInjectionProvider.GetService<SchemaConfigViewModel>();
        //    if (schemaConfigViewModel == null)
        //    {
        //        return;
        //    }
        //    schemaConfigViewModel.PropertyChanged -= SchemaConfigViewModel_PropertyChanged;
        //}

        //private void SchemaConfigViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (sender is SchemaConfigViewModel schemaConfigViewModel && e.PropertyName == nameof(SchemaConfigViewModel.Enabled))
        //    {
        //        if (ButtonSchema.Flyout is { } && !schemaConfigViewModel.Enabled)
        //        {
        //            ButtonSchema.Flyout.Hide();
        //        }
        //    }
        //}
    }
}
