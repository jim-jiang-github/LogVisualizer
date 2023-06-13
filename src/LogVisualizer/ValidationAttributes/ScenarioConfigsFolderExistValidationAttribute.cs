using LogVisualizer.Commons;
using LogVisualizer.Services;
using Metalama.Framework.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ValidationAttributes
{
    public class ScenarioConfigsFolderExistValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            var gitService = DependencyInjectionProvider.GetService<GitService>();
            if (value is string folderName)
            {
                string folder = System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, folderName);
                if (!Directory.Exists(folder))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
