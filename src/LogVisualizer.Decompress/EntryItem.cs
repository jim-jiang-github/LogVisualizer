using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Decompress
{
    internal class EntryItem
    {
        public string EntryPath { get; }
        public Stream EntryItemStream { get; }

        public EntryItem(string entryPath, Stream entryItemStream)
        {
            EntryPath = entryPath;
            EntryItemStream = entryItemStream;
        }
    }
}
