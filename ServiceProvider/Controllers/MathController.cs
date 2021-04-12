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
        public MathResult AddTwoNumbers(MathInput input)
        {
            if (input.Values.Count != 2)
                return new MathResult(-1, false, "bad number of parameters");

            return new MathResult(input.Values[0] + input.Values[1]); 
        }

        [Route("api/add3")]
        [HttpPost]
        public MathResult AddThreeNumbers(MathInput input)
        {
            if (input.Values.Count != 3)
                return new MathResult(-1, false, "bad number of parameters");

            return new MathResult(input.Values[0] + input.Values[1] + input.Values[2]);
        }

        [Route("api/multiply2")]
        [HttpPost]
        public MathResult MultiplyTwoNumbers(MathInput input)
        {
            if (input.Values.Count != 2)
                return new MathResult(-1, false, "bad number of parameters");

            return new MathResult(input.Values[0] * input.Values[1]);
        }

        [Route("api/multiply3")]
        [HttpPost]
        public MathResult MultiplyThreeNumbers(MathInput input)
        {
            if (input.Values.Count != 3)
                return new MathResult(-1, false, "bad number of parameters");

            return new MathResult(input.Values[0] * input.Values[1] * input.Values[2]);
        }

        [Route("api/prime-to")]
        [HttpPost]
        public MathListResult GetPrimesTo(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathListResult(null, false, "bad number of parameters");

            try
            {
                return new MathListResult(MathModel.GeneratePrimesUpTo(input.Values[0])); //TODO catch ArgumentException
            }
            catch (ArgumentException a)
            {
                return new MathListResult(null, false, "calculation error: " + a.Message);
            }
        }

        [Route("api/prime-range")]
        [HttpPost]
        public MathListResult GetPrimesInRange(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathListResult(null, false, "bad number of parameters");

            try
            {
                return new MathListResult(MathModel.GeneratePrimesBetween(input.Values[0], input.Values[1])); //TODO catch ArgumentException
            }
            catch (ArgumentException a)
            {
                return new MathListResult(null, false, "calculation error: " + a.Message);
            }
        }

        [Route("api/is-prime")]
        [HttpPost]
        public MathBooleanResult IsPrimeNumber(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathBooleanResult(false, false, "bad number of parameters");

            return new MathBooleanResult(MathModel.IsPrime(input.Values[0]));
        }
    }
}