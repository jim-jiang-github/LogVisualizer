using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Threading;
using LogVisualizer.Commons;
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
    }
}