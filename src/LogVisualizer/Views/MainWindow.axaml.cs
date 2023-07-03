using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Interactivity;
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
    }
}