using APIClasses.Math;
using ServiceProvider.Models;
using System;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    public class MathController : ApiController
    {
        /// <summary>
        /// Adds 2 numbers together and returns the result
        /// </summary>
        /// <param name="input">Math input containing 2 numbers to add</param>
        /// <returns>Sum of 3 numbers</returns>
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

        /// <summary>
        /// Adds 3 numbers together and returns the result
        /// </summary>
        /// <param name="input">Math input containing 3 numbers to add</param>
        /// <returns>Sum of 3 numbers</returns>
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

        /// <summary>
        /// Multiplies 2 numbers together and returns the result
        /// </summary>
        /// <param name="input">Math input containing 2 numbers to multiply</param>
        /// <returns>Product of 2 numbers</returns>
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

        /// <summary>
        /// Multiplies 3 numbers together and returns the result
        /// </summary>
        /// <param name="input">Math input containing 3 numbers to multiply</param>
        /// <returns>Product of 3 numbers</returns>
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

        /// <summary>
        /// Gets a list of prime numbers up to the given value (inclusive).
        /// </summary>
        /// <param name="input">Upper limit of list of prime numbers to return (inclusive)</param>
        /// <returns>List of prime numbers</returns>
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

        /// <summary>
        /// Gets a list of prime numbers between the two given values (inclusive).
        /// </summary>
        /// <param name="input">Lower and upper limit of list of prime numbers to return(inclusive)</param>
        /// <returns>List of prime numbers</returns>
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

        /// <summary>
        /// Returns the input number is prime
        /// </summary>
        /// <param name="input">Number to check prime status of</param>
        /// <returns>Prime status of number</returns>
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