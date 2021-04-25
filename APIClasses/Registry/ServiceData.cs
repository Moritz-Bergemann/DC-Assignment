using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses.Registry
{
    /// <summary>
    /// Class containing data representing a service. Used for API requests & for passing service information.
    /// </summary>
    public class ServiceData
    {
        public string Name;
        public string Description;
        public string ApiEndpoint;
        public int NumOperands;
        public string OperandType;
    }
}
