using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using LogVisualizer.Commons;
using LogVisualizer.Scenarios;
using LogVisualizer.Scenarios.Schemas;
using LogVisualizer.Services;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reactive.Concurrency;
using System.Text.RegularExpressions;

namespace LogVisualizer.Test
{
    public class SchemaTest
    {
        public SchemaTest()
        {
        }

        [Fact]
        public void SchemaLogText()
        {
            SchemaLogText schemaLogText = new SchemaLogText();
            var path = schemaLogText.Default_22_2_20();
        }
    }
}