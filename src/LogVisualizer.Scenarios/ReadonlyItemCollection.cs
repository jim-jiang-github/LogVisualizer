using LogVisualizer.Scenarios.Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios
{
    internal class ReadonlyItemCollection : IList
    {
        private ContentSource _contentSource;
        public ReadonlyItemCollection(ContentSource contentSource)
        {
            _contentSource = contentSource;
        }

        public object? this[int index]
        {
            get
            {
                return _contentSource.GetRow(index);
            }
            set { }
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public int Count => _contentSource.RowCount;

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
}
