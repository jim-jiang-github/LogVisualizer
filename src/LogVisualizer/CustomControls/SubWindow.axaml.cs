using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using LogVisualizer.Platforms.Windows;

namespace LogVisualizer.CustomControls
{
    public partial class SubWindow : UserControl
    {
        private bool isDragging;
        private Point startPoint;
        private TranslateTransform transform;

        public static readonly StyledProperty<bool> IsModalDialogProperty = AvaloniaProperty.Register<SubWindow, bool>(nameof(IsModalDialog), true);
        public static readonly StyledProperty<double> WindowWidthProperty = AvaloniaProperty.Register<SubWindow, double>(nameof(WindowWidth), 450);
        public static readonly StyledProperty<double> WindowHeightProperty = AvaloniaProperty.Register<SubWindow, double>(nameof(WindowHeight), 600);
        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<SubWindow, string>(nameof(Title), "Title");

        public bool IsModalDialog
        {
            get => GetValue(IsModalDialogProperty);
            set => SetValue(IsModalDialogProperty, value);
        }
        public double WindowWidth
        {
            get { return GetValue(WindowWidthProperty); }
            set { SetValue(WindowWidthProperty, value); }
        }
        public double WindowHeight
        {
            get { return GetValue(WindowHeightProperty); }
            set { SetValue(WindowHeightProperty, value); }
        }
        public string Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public SubWindow()
        {
            InitializeComponent();
            IsVisible = false;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var subWindowContainer = e.NameScope.Find<DockPanel>("SubWindowContainer");
            if (subWindowContainer != null)
            {
                transform = new TranslateTransform();
                subWindowContainer.RenderTransform = transform;
                subWindowContainer.PointerPressed += SubWindowContainer_PointerPressed;
                subWindowContainer.PointerReleased += SubWindowContainer_PointerReleased;
                subWindowContainer.PointerMoved += SubWindowContainer_PointerMoved;
            }
        }

        private void SubWindowContainer_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Visual? visual = sender as Visual;
            if (visual == null)
            {
                return;
            }
            if (e.GetCurrentPoint(visual).Properties.IsLeftButtonPressed)
            {
                isDragging = true;
                startPoint = e.GetPosition(this);
                e.Handled = true;
            }
        }

        private void SubWindowContainer_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            Visual? visual = sender as Visual;
            if (visual == null)
            {
                return;
            }
            if (e.GetCurrentPoint(visual).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                isDragging = false;
                e.Handled = true;
            }
        }

        private void SubWindowContainer_PointerMoved(object? sender, PointerEventArgs e)
        {
            Visual? visual = sender as Visual;
            if (visual == null)
            {
                return;
            }
            if (isDragging)
            {
                var parent = visual.GetVisualParent<Visual>();
                var endPoint = e.GetPosition(parent);

                double offsetX = endPoint.X - startPoint.X;
                double offsetY = endPoint.Y - startPoint.Y;

                transform.X = offsetX;
                transform.Y = offsetY;
                e.Handled = true;
            }
        }
    }
}
