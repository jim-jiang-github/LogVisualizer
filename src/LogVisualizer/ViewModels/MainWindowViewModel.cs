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
    public struct Item
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class MainWindowViewModel : ViewModelBase
    {
        public ItemCollection Items { get; }
        public string Greeting => "Welcome to Avalonia!";
        public MainWindowViewModel()
        {
            ItemCollection itemCollection = new ItemCollection();
            //var items = new Item[999999];
            //for (int i = 0; i < items.Length; i++)
            //{
            //    items[i].Name = $"xxxxxxxxxxxxxxxxxxxxxxxx{i}";
            //    items[i].Age = i;
            //}
            Items = itemCollection;
        }
    }
    public class ItemCollection
    {
        public string[] Headers { get; } = new string[] { "Name", "Age", "Detail", "Msg" };
        public string[][] Content { get; }

        public ItemCollection()
        {
            Content = new string[9999999][];
            for (int i = 0; i < Content.Length; i++)
            {
                Content[i] = new string[]
                {
                    $"Name{i}",
                    i.ToString(),
                    $"Detail{i}",
                    $"Ms{i}",
                };
            }
        }
    }
}