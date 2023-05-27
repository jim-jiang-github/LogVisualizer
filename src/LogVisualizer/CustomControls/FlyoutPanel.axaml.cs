using Avalonia;
using Avalonia.Controls;

namespace LogVisualizer.CustomControls
{
    public partial class FlyoutPanel : UserControl
    {
        public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<FlyoutPanel, string?>(nameof(Title), "Title");
        public static readonly StyledProperty<double> PanelWidthProperty = AvaloniaProperty.Register<SubWindow, double>(nameof(PanelWidth), 500);
        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public double PanelWidth
        {
            get { return GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }

        public FlyoutPanel()
        {
            InitializeComponent();
        }
    }
}
