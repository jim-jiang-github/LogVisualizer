using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using LogVisualizer.Commons;
using LogVisualizer.CustomControls;
using LogVisualizer.Messages;
using LogVisualizer.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace LogVisualizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected async override void OnLoaded()
        {
            base.OnLoaded();
            Notify.Init(this);
            WeakReferenceMessenger.Default.Register<LogFilterItemDetailSelectedChangedMessage>(this, (r, m) =>
            {
                LogFilterItemEditor logFilterItemEditor = new LogFilterItemEditor();
                var logFilterItemEditorViewModel = logFilterItemEditor.DataContext as LogFilterItemEditorViewModel;
                if (logFilterItemEditorViewModel == null)
                {
                    Log.Warning("LogFilterItemEditor.DataContext is not LogFilterItemEditorViewModel or is null.");
                    return;
                }
                logFilterItemEditorViewModel.LogFilterItem = m.Value;
                SubDialogWindow subDialogWindow = new SubDialogWindow()
                {
                    Content = logFilterItemEditor
                };
                subDialogWindow.ShowDialog(this);
            });
        }
    }
}