using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.ValidationAttributes
{
    public class ValueRequiredAttribute : ValidationAttribute
    {
        public ValueRequiredAttribute()
        {
        }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return value.ToString() != string.Empty;
        }
    }
}
