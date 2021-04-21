using APIClasses.Math;
using ServiceProvider.Models;
using System;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    public class MathController : ApiController
    {
        [Route("api/add2")]
        [HttpPost]
        public MathResponse AddTwoNumbers(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {
                if (input.Values.Count != 2)
                    return new MathResponse(null, false, "bad number of parameters");

                return new MathResponse(input.Values[0] + input.Values[1]);
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/add3")]
        [HttpPost]
        public MathResponse AddThreeNumbers(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {

                if (input.Values.Count != 3)
                    return new MathResponse(null, false, "bad number of parameters");

                return new MathResponse(input.Values[0] + input.Values[1] + input.Values[2]);
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/multiply2")]
        [HttpPost]
        public MathResponse MultiplyTwoNumbers(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {
                if (input.Values.Count != 2)
                    return new MathResponse(null, false, "bad number of parameters");

                return new MathResponse(input.Values[0] * input.Values[1]);
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/multiply3")]
        [HttpPost]
        public MathResponse MultiplyThreeNumbers(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {
                if (input.Values.Count != 3)
                    return new MathResponse(null, false, "bad number of parameters");

                return new MathResponse(input.Values[0] * input.Values[1] * input.Values[2]);
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/prime-to")]
        [HttpPost]
        public MathResponse GetPrimesTo(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {
                if (input.Values.Count != 1)
                    return new MathResponse(null, false, "bad number of parameters");

                try
                {
                    return new MathResponse(MathModel.GeneratePrimesUpTo(input.Values[0]));
                }
                catch (ArgumentException a)
                {
                    return new MathResponse(null, false, "calculation error: " + a.Message);
                }
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/prime-range")]
        [HttpPost]
        public MathResponse GetPrimesInRange(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {

                if (input.Values.Count != 1)
                    return new MathResponse(null, false, "bad number of parameters");

                try
                {
                    return new MathResponse(MathModel.GeneratePrimesBetween(input.Values[0], input.Values[1]));
                }
                catch (ArgumentException a)
                {
                    return new MathResponse(null, false, "calculation error: " + a.Message);
                }
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }

        [Route("api/is-prime")]
        [HttpPost]
        public MathResponse IsPrimeNumber(IntegerMathRequest input)
        {
            if (MathModel.Instance.CheckAuthentication(input.Token))
            {
                if (input.Values.Count != 1)
                    return new MathResponse(null, false, "bad number of parameters");

                return new MathResponse(MathModel.IsPrime(input.Values[0]).ToString());
            }
            else
            {
                return new MathResponse(false, "Authentication Error");
            }
        }
    }
}