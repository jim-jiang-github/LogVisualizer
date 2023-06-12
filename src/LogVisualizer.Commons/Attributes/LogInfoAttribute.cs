using Metalama.Framework.Aspects;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Commons.Attributes
{
    public class LogInfoAttribute : OverrideMethodAspect
    {
        public override dynamic? OverrideMethod()
        {
            Log.Information(meta.Target.Method.ToDisplayString() + " Enter.");
            var result = meta.Proceed();
            Log.Information(meta.Target.Method.ToDisplayString() + " Leave.");
            return result;
        }
    }
}
