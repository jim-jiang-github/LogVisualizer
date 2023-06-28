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

        protected override async void OnLoaded()
        {
            base.OnLoaded();
            Notify.Init(this);
            WeakReferenceMessenger.Default.Register<LogFilterItemDetailSelectedChangedMessage>(this, async (r, m) =>
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
                await subDialogWindow.ShowDialog(this);
                m.SetResponse(true);
            });
            //LogRowDetail logRowDetail = new LogRowDetail();
            //var logRowDetailViewModel = logRowDetail.DataContext as LogRowDetailViewModel;
            //if (logRowDetailViewModel == null)
            //{
            //    Log.Warning("LogRowDetail.DataContext is not LogRowDetailViewModel or is null.");
            //    return;
            //}
            //logRowDetailViewModel.SetLogRow(new[] { "asdasd", "qweqwe", "dfgdfgdfg" }, new Scenarios.Contents.LogRow(0, new[] { "111", "222", "333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333\r\n333333333333333333333333333333333333333333333333333333333333333\r\n33333333333333333333333333333333333333333333333333333333333333333333333333333" }));

            //SubDialogWindow subDialogWindow1 = new SubDialogWindow()
            //{
            //    Content = logRowDetail
            //};
            //await subDialogWindow1.ShowDialog(this);
        }
    }
}