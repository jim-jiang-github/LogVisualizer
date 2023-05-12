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
    public partial class SideBarViewModel : ViewModelBase
    {
        public partial class SideBarItem : ViewModelBase
        {
            [ObservableProperty]
            private string? _name;
            [ObservableProperty]
            private string? _tip;
            [ObservableProperty]
            private string? _resourceKey;
        }

        [ObservableProperty]
        private ObservableCollection<SideBarItem> _sideBarItems;
        public SideBarViewModel(SideBarService sideBarService)
        {
            _sideBarItems = new ObservableCollection<SideBarItem>();
            for (int i = 0; i < 3; i++)
            {
                SideBarItems.Add(new SideBarItem()
                {
                    Tip = i == 0 ? "xxxxxxxxxxx" : null,
                    Name = i.ToString(),
                    ResourceKey = "book"
                });
            }
        }

        [RelayCommand]
        public void A() { }
    }
}
