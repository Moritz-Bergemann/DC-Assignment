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

    public class PublishRequest : SecureRequest
    {
        public ServiceData Data;

        public PublishRequest(int token, ServiceData data) : base(token)
        {
            Data = data;
        }
    }

    public class UnpublishRequest : SecureRequest
    {
        public string ApiEndpoint;

        public UnpublishRequest(int token, string apiEndpoint) : base(token)
        {
            ApiEndpoint = apiEndpoint;
        }
    }

    public class SearchRequest : SecureRequest
    {
        public string Query;

        public SearchRequest(int token, string query) : base(token)

        {
            Query = query;
            Token = token;
        }
    }

    public class SearchResult : SecureResponse
    {
        public List<ServiceData> Values;

        public SearchResult() : base()
        { }

        public SearchResult(bool accepted, string acceptReason, List<ServiceData> values) : base(accepted, acceptReason)

        {
            Values = values;
        }
    }

    public class PublishResult: SecureResponse
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
