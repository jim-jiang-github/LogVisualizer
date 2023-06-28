using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace LogVisualizer.CustomControls
{
    public class AutoGridPanel : StackPanel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var width = Children.Sum(x =>
            {
                x.Measure(availableSize);
                return x.DesiredSize.Width;
            });
            if (width < availableSize.Width)
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal;
            }
            else
            {
                Orientation = Avalonia.Layout.Orientation.Vertical;
            }
            return base.MeasureOverride(availableSize);
        }
    }
}
