using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public partial class BottomBarViewModel : ViewModelBase
    {
        public SchemaConfigViewModel SchemaConfig { get; }

        public BottomBarViewModel(SchemaConfigViewModel schemaConfigViewModel)
        {
            SchemaConfig = schemaConfigViewModel;
        }
    }
}
