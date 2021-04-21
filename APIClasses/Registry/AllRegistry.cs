using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using APIClasses.Security;

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

        public int Token;

        public SearchData(int token, string query)
        {
            Query = query;
            Token = token;

        }
    }

    public class SearchResponse
    {
        public List<RegistryData> Values;

        public string Status;
        public string Reason;

        public SearchResponse()
        { }

        public SearchResponse(bool accepted, string acceptReason, List<RegistryData> values)
        {
            Values = values;
            Status = accepted ? "Accepted" : "Denied";
            Reason = acceptReason;
        }
    }

    public class PublishResult
    {
        public bool Success;
        public string Message;

        public string Status;
        public string Reason;

        public PublishResult()
        { }
        public PublishResult(bool accepted, string reason, bool success, string message)
        {
            Success = success;
            Message = message;

            Status = accepted ? "Accepted" : "Denied";
            Reason = reason;

        }
    }
}
