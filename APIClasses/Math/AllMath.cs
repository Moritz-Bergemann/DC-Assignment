using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace APIClasses.Math
{
    //TODO - move all of these out into separate classes
    public class MathResult 
    {
        public string Result;
        public bool Success;
        public string Message;

        //Required empty constructor
        public MathResult()
        { }

        public MathResult(int result)
        {
            Result = result.ToString();
            Success = true;
            Message = "success";
        }

        public MathResult(List<int> result)
        {
            Result = string.Join(", ", result);
            Success = true;
            Message = "success";
        }

        public MathResult(bool result)
        {
            Result = result.ToString();
            Success = true;
            Message = "success";
        }

        public MathResult(string result)
        {
            Result = result;
            Success = true;
            Message = "success";
        }

        public MathResult(string result, bool success, string message)
        {
            Result = result;
            Success = success;
            Message = message;
        }
    }
    public class MathInput
    {
        public List<int> Values = new List<int>();
    }
}
