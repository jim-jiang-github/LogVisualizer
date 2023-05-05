using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogVisualizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ItemList _items;
        public ItemList Items => _items ?? new ItemList();
        public string Greeting => "Welcome to Avalonia!";
        public MainWindowViewModel()
        {
        }

        
    }
    public class ItemList : IList
    {
        public object this[int index]
        {
            get
            {
                return $"xxxxxxxxxxxxxxxxxxxxxxxxxxx{index}";
            }
            set => throw new NotImplementedException();
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public int Count => int.MaxValue;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        public int Add(object value)
        {
            return -1;
        }

        public void Clear()
        {

        }

        public bool Contains(object value)
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

        public int IndexOf(object value)
        {
            return -1;
        }

        public void Insert(int index, object value)
        {
        }

        public void Remove(object value)
        {
        }

        public void RemoveAt(int index)
        {
        }
    }

}