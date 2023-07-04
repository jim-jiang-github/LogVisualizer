using Avalonia;
using Avalonia.Controls;
using System.Linq;

namespace LogVisualizer.CustomControls
{
    public partial class FlyoutPanel : UserControl
    {
        public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<FlyoutPanel, string?>(nameof(Title), null);
        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public FlyoutPanel()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var child = VisualChildren.FirstOrDefault() as Control;
            if (child == null)
            {
                return base.MeasureOverride(availableSize);
            }
            child.Measure(availableSize);
            return base.MeasureOverride(child.DesiredSize);
        }
    }
}
