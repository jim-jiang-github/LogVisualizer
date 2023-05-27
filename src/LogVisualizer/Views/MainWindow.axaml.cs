using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Threading;
using LogVisualizer.Commons.Notifications;
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
        }
        private bool flag = false;
        protected async override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Task.Run(async () =>
            {
                await Notify.ShowMessageBox("xxxx","asdasd");
            });
            return;
            if (flag)
            {
                I18N.I18NManager.CurrentCulture = CultureInfo.GetCultureInfo("zh-CN");
            }
            else
            {
                I18N.I18NManager.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            flag = !flag;
        }
    }
}