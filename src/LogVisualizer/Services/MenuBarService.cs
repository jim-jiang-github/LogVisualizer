using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class MenuBarService
    {
        public class MenuItem
        {
            public string Name { get; }
            public string Icon { get; }
            public string Gesture { get; }
            public IEnumerable<MenuItem> SubItems { get; }

            public MenuItem(string name, string icon, string gesture, IEnumerable<MenuItem> subItems)
            {
                Name = name;
                Icon = icon;
                Gesture = gesture;
                SubItems = subItems;
            }
        }

    }
}
