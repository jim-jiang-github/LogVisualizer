using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class RoslynHelper
    {
        public static T? TryCreate<T>(string csharpCode, Assembly callAssembly, out Exception? exception)
        {
            exception = null;
            Type type = typeof(T);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);
            Compilation compilation = CSharpCompilation.Create("ConsoleApp2")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(syntaxTree);

            var refferences = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(callAssembly.Location),
            };

            foreach (var ass in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                refferences.Add(MetadataReference.CreateFromFile(Assembly.Load(ass).Location));
            }
            compilation.AddReferences(refferences);
            Assembly assembly;
            using var memoryStream = new MemoryStream();
            var result = compilation.Emit(memoryStream);
            if (!result.Success)
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);
                string errors = string.Join(Environment.NewLine, failures.Select(x => x.GetMessage()));
                exception = new Exception("Compilation failed: " + errors);
                return default(T);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            assembly = Assembly.Load(memoryStream.ToArray());

            Type? implementationType = assembly.GetTypes().FirstOrDefault(t => t.GetInterfaces().Contains(type));
            if (implementationType == null)
            {
                exception = new Exception("Implementation not found!");
                return default(T);
            }
            T instance = (T)Activator.CreateInstance(implementationType);
            if (implementationType == null)
            {
                exception = new Exception("Cannot create instance!");
                return default(T);
            }
            return instance;
        }
    }
}
