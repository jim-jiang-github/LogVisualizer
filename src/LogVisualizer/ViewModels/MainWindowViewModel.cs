using Avalonia.Controls;
using DataVirtualization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LogVisualizer.ViewModels
{
    public class Item
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class MainWindowViewModel : ViewModelBase
    {
        public VirtualizingCollection<Item> Items { get; }
        public string Greeting => "Welcome to Avalonia!";
        public MainWindowViewModel()
        {
            var customerProvider = new CustomerProvider();
            var customerList = new VirtualizingCollection<Item>(customerProvider, 30, 3000);
            Items = customerList;
        }


    }
    public class CustomerProvider : IItemsProvider<Item>
    {
        private Item[] _items = new Item[30];

        public int FetchCount()
        {
            return 30;
        }

        public IList<Item> FetchRange(int startIndex, int pageCount, out int overallCount)
        {
            overallCount = 3000;
            for (int i = 0; i < pageCount; i++)
            {
                _items[i] = new Item()
                {
                    Age = startIndex,
                    Name = $"xxxxxxx{startIndex}"
                };
            }
            return _items;
        }
    }
}