using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    public class FilterCompariser
    {
        public enum ComparisonTypes { AreEqual, AreNotEqual, GreatherOrEqual, GreatherThan, LessThan, LessOrEqual}

        public static ComparisonTypes ComparisonType { get; set; }


    }
}
