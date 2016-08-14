using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlMaker;

namespace Reflection
{
    public class NazwaAttribute : DisplayNameAttribute
    {
        public NazwaAttribute(string resourceName)
            : base(GetMessageFromResource(resourceName))
        { }

        private static string GetMessageFromResource(string resourceId)
        {
            return ControlNames.ResourceManager.GetString(resourceId);
        }
    }
}
