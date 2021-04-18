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
                return new MathResult(null, false, "bad number of parameters");

            return new MathResult(input.Values[0] + input.Values[1]); 
        }

        [Route("api/add3")]
        [HttpPost]
        public MathResult AddThreeNumbers(MathInput input)
        {
            if (input.Values.Count != 3)
                return new MathResult(null, false, "bad number of parameters");

            return new MathResult(input.Values[0] + input.Values[1] + input.Values[2]);
        }

        [Route("api/multiply2")]
        [HttpPost]
        public MathResult MultiplyTwoNumbers(MathInput input)
        {
            if (input.Values.Count != 2)
                return new MathResult(null, false, "bad number of parameters");

            return new MathResult(input.Values[0] * input.Values[1]);
        }

        [Route("api/multiply3")]
        [HttpPost]
        public MathResult MultiplyThreeNumbers(MathInput input)
        {
            if (input.Values.Count != 3)
                return new MathResult(null, false, "bad number of parameters");

            return new MathResult(input.Values[0] * input.Values[1] * input.Values[2]);
        }

        [Route("api/prime-to")]
        [HttpPost]
        public MathResult GetPrimesTo(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathResult(null, false, "bad number of parameters");

            try
            {
                return new MathResult(MathModel.GeneratePrimesUpTo(input.Values[0])); //TODO catch ArgumentException
            }
            catch (ArgumentException a)
            {
                return new MathResult(null, false, "calculation error: " + a.Message);
            }
        }

        [Route("api/prime-range")]
        [HttpPost]
        public MathResult GetPrimesInRange(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathResult(null, false, "bad number of parameters");

            try
            {
                return new MathResult(MathModel.GeneratePrimesBetween(input.Values[0], input.Values[1])); //TODO catch ArgumentException
            }
            catch (ArgumentException a)
            {
                return new MathResult(null, false, "calculation error: " + a.Message);
            }
        }

        [Route("api/is-prime")]
        [HttpPost]
        public MathResult IsPrimeNumber(MathInput input)
        {
            if (input.Values.Count != 1)
                return new MathResult(null, false, "bad number of parameters");

            return new MathResult(MathModel.IsPrime(input.Values[0]));
        }
    }
}