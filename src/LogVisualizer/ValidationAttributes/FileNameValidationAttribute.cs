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
    public class FileNameValidationAttribute : RegularExpressionAttribute
    {
        public FileNameValidationAttribute() : base(@"^[^<>:""/\\|?*\x00-\x1F\x7F]+(\.[^<>:""/\\|?*\x00-\x1F\x7F]+)*$")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }
            return base.IsValid(value);
        }
    }
}
