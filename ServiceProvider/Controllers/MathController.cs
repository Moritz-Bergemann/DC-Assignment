using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIClasses.Math;
using ServiceProvider.Models;

namespace ServiceProvider.Controllers
{
    public class MathController : ApiController
    {
        //TODO proper error handling

        [Route("api/add2")]
        [HttpPost]
        public MathResult AddTwoNumbers(TwoValueInput input)
        {
            return new MathResult(input.Value1 + input.Value2); 
        }

        [Route("api/add3")]
        [HttpPost]
        public MathResult AddThreeNumbers(ThreeValueInput input)
        {
            return new MathResult(input.Value1 + input.Value2 + input.Value3);
        }

        [Route("api/multiply2")]
        [HttpPost]
        public MathResult MultiplyTwoNumbers(TwoValueInput input)
        {
            return new MathResult(input.Value1 * input.Value2);
        }

        [Route("api/multiply3")]
        [HttpPost]
        public MathResult MultiplyThreeNumbers(ThreeValueInput input)
        {
            return new MathResult(input.Value1 * input.Value2 * input.Value3);
        }

        [Route("api/prime-to")]
        [HttpPost]
        public List<int> GetPrimesTo(OneValueInput input)
        {
            return MathModel.GeneratePrimesUpTo(input.Value); //TODO catch ArgumentException
        }

        [Route("api/prime-range")]
        [HttpPost]
        public List<int> GetPrimesInRange(TwoValueInput input)
        {
            return MathModel.GeneratePrimesBetween(input.Value1, input.Value2); //TODO catch ArgumentException
        }

        [Route("api/is-prime")]
        [HttpPost]
        public BooleanResult IsPrimeNumber(OneValueInput input)
        {
            return new BooleanResult(MathModel.IsPrime(input.Value));
        }
    }
}