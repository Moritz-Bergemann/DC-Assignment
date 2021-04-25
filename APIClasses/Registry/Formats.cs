using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses.Registry
{
    /// <summary>
    /// Static class for storing permitted formats in registry.
    /// </summary>
    public class Formats
    {
        public static string[] AllowedOperandTypes = { "integer" }; //NOTE - currently only integer supported, but more can be added here
    }
}
