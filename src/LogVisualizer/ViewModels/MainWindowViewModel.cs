using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using Avalonia;
using System.Collections;
using LogVisualizer.Services;

namespace LogVisualizer.ViewModels
{
    public struct Item
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ItemCollection : IList
    {
        public object? this[int index]
        {
            get
            {
                return new Item()
                {
                    Index = index,
                    Name = $"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx{index}",
                    Age = index
                };
            }
            set => throw new NotImplementedException();
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public int Count => 333;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        public int Add(object? value)
        {
            return -1;
        }

        public void Clear()
        {
        }

        public bool Contains(object? value)
        {
            return false;
        }

        public void CopyTo(Array array, int index)
        {
        }

        public IEnumerator GetEnumerator()
        {
            yield return -1;
        }

        public int IndexOf(object? value)
        {
            return -1;
        }

        public void Insert(int index, object? value)
        {
        }

        public void Remove(object? value)
        {
        }

        public void RemoveAt(int index)
        {
        }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        public SideBarViewModel SideBar { get; }
        public UpgraderViewModel Upgrader { get; }
        public ItemCollection Items { get; }

#if DEBUG
        public MainWindowViewModel()
        {

        }
#endif

        public MainWindowViewModel(SideBarViewModel sideBarViewModel, UpgraderViewModel upgraderViewModel)
        {
            SideBar = sideBarViewModel;
            Upgrader = upgraderViewModel;
            Items = new ItemCollection();
        }
    }

}