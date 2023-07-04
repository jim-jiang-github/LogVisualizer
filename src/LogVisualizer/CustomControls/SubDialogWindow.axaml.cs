using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Commons;
using LogVisualizer.I18N;
using LogVisualizer.Models;
using LogVisualizer.Services;
using MessageBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LogVisualizer.CustomControls
{
    public partial class SubDialogWindow : Window
    {
        public static readonly StyledProperty<ObservableCollection<MessageBoxButton>> ButtonsProperty = AvaloniaProperty.Register<SubDialogWindow, ObservableCollection<MessageBoxButton>>(nameof(Buttons), new ObservableCollection<MessageBoxButton>());

        public ObservableCollection<MessageBoxButton> Buttons
        {
            get { return GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        private MessageBoxButton? _clickButton = null;

        public SubDialogWindow()
        {
            InitializeComponent();
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
        }

        [RelayCommand]
        private void ClickButton(MessageBoxButton button)
        {
            _clickButton = button;
            if (button.CloseOnClick)
            {
                Close();
            }
        }

        public async Task<string?> ShowDialogAsync(Window ownerWindow)
        {
            await ShowDialog(ownerWindow);
            return _clickButton?.ButtonText;
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
