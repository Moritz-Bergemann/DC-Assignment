using System.Collections.Generic;
using System.Runtime.InteropServices;
using APIClasses.Security;

namespace APIClasses.Math
{
    public class MathResponse : SecureResponse
    {
        public string Result;
        public bool Success;
        public string Message;

        //Required empty constructor
        public MathResponse()
        { }

        //Constructors for quick conversion of data types to string (assumes authentication was accepted)
        public MathResponse(int result) : base(true, null)
        {
            Result = result.ToString();
            Success = true;
            Message = "success";
        }

        public MathResponse(List<int> result) : base(true, null)
        {
            Result = string.Join(", ", result);
            Success = true;
            Message = "success";
        }

        public MathResponse(string result) : base(true, null)
        {
            Result = result;
            Success = true;
            Message = "success";
        }

        public MathResponse(string result, bool success, string message) : base(true, null)
        {
            Result = result;
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Constructor for authentication failure
        /// </summary>
        /// <param name="authenticationStatus">Authentication status - true if accepted, false if denied</param>
        /// <param name="reason">Reason for authentication status denial (null if status was accepted)</param>
        public MathResponse(bool authenticationStatus, string reason) : base(authenticationStatus, reason)
        {
            Success = false;
            Result = null;
        }
    }
    public class IntegerMathRequest : SecureRequest
    {
        public List<int> Values = new List<int>();

        public IntegerMathRequest(int token, List<int> values) : base(token)
        {
            Values = values;
        }
    }
}
