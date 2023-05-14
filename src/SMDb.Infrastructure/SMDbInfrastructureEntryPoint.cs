using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SMDb.Infrastructure
{
    public static class SMDbInfrastructureEntryPoint
    {
        public static Assembly Assembly => typeof(SMDbInfrastructureEntryPoint).Assembly;
    }
}
