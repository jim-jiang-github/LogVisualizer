﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Scenarios.Commons
{
    //https://referencesource.microsoft.com/#mscorlib/system/io/filesystemenumerable.cs,a745dc38ae585a3f
    // Abstract Iterator, borrowed from Linq. Used in anticipation of need for similar enumerables
    // in the future
#nullable disable
    abstract internal class Iterator<TSource> : IEnumerable<TSource>, IEnumerator<TSource>
    {
        int threadId;
        internal int state;
        protected TSource current;

        public Iterator()
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public TSource Current
        {
            get { return current; }
        }

        protected abstract Iterator<TSource> Clone();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            current = default(TSource);
            state = -1;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            if (threadId == Thread.CurrentThread.ManagedThreadId && state == 0)
            {
                state = 1;
                return this;
            }

            Iterator<TSource> duplicate = Clone();
            duplicate.state = 1;
            return duplicate;
        }

        public abstract bool MoveNext();

        object IEnumerator.Current
        {
            get { return Current; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }
    }
#nullable enable
}
