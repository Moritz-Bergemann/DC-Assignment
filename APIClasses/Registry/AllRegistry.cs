using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses.Registry
{
    public class RegistryData
    {
        public string Name;
        public string Description;
        public string ApiEndpoint;
        public int NumOperands;
        public string OperandType;
    }

    public class EndpointData
    {
        public string ApiEndpoint;
    }

    public class SearchData
    {
        public string Query;

        public SearchData(string query)
        {
            Query = query;
        }
    }

    public class PublishResult
    {
        public bool Success;
        public string Message;

        public PublishResult()
        { }
        public PublishResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
