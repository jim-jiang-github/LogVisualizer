using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using Avalonia;
using System.Collections;
using LogVisualizer.Services;
using LogVisualizer.Views;
using LogVisualizer.Scenarios;

namespace LogVisualizer.ViewModels
{
    public class LogDisplayViewModel : ViewModelBase
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
                        Name = $"{index}xxx",
                        Age = index
                    };
                }
                set => throw new NotImplementedException();
            }

            public bool IsFixedSize => true;

            public bool IsReadOnly => true;

            public int Count => int.MaxValue;

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
        public IList? Items { get; }

        public LogDisplayViewModel()
        {
            Items = new ItemCollection();
        }
    }

}