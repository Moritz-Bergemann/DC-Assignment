using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using APIClasses.Security;

namespace APIClasses.Registry
{
    public class ServiceData
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

    public class SearchData : SecureRequest
    {
        public string Query;

        public SearchData(int token, string query) : base(token)
        {
            Query = query;
        }
    }

    public class SearchResponse : SecureResponse
    {
        public List<ServiceData> Values;

        public SearchResponse() : base()
        { }

        public SearchResponse(bool accepted, string acceptReason, List<ServiceData> values) : base(accepted, acceptReason)
        {
            Values = values;
        }
    }

    public class PublishResult : SecureResponse
    {
        public bool Success;
        public string Message;

        public PublishResult() : base()
        { }
        public PublishResult(bool accepted, string reason, bool success, string message) : base(accepted, reason)
        {
            Success = success;
            Message = message;
        }
    }
}
