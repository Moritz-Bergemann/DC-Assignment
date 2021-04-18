﻿using System.Collections.Generic;
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
            Result = result.ToString();
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

    //public class MathResult
    //{
    //    public int Result;
    //    public bool Success;
    //    public string Message;

    //    public MathResult(int result)
    //    {
    //        Result = result;
    //        Success = true;
    //        Message = "success";
    //    }

    //    public MathResult(int result, bool success, string message)
    //    {
    //        Result = result;
    //        Success = success;
    //        Message = message;
    //    }
    //}

    //public class MathListResult
    //{
    //    public List<int> Results;
    //    public bool Success;
    //    public string Message;

    //    public MathListResult(List<int> results, bool success, string message)
    //    {
    //        Results = results;
    //        Success = success;
    //        Message = message;
    //    }
    //    public MathListResult(List<int> results)
    //    {
    //        Results = results;
    //        Success = true;
    //        Message = "success";
    //    }
    //}

    //public class MathBooleanResult
    //{
    //    public bool Result;
    //    public bool Success;
    //    public string Message;

    //    public MathBooleanResult(bool result, bool success, string message)
    //    {
    //        Result = result;
    //        Success = success;
    //        Message = message;
    //    }
    //    public MathBooleanResult(bool result)
    //    {
    //        Result = result;
    //        Success = true;
    //        Message = "success";
    //    }
    //}
}
