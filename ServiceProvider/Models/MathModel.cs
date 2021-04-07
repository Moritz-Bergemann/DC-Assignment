using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ServiceProvider.Models
{
    public class MathModel
    {
        /// <summary>
        /// Generates a list of all prime numbers from 2 up to the given upper bound (inclusive).
        /// </summary>
        /// <param name="limit"></param> upper bound
        /// <returns></returns> prime number list
        public static List<int> GeneratePrimesUpTo(int limit)
        {
            if (limit < 2)
            {
                return new List<int>();
            }

            List<int> primes = new List<int>();
            
            //Add 2 to get us started
            primes.Add(2);

            for (int ii = 3; ii <= limit; ii++) //Check each number
            {
                //Check if number is evenly divisible by any prime number we've found so far (and therefore divisible by any number)
                bool divisible = primes.Any(primeNum => ii % primeNum == 0);

                if (!divisible)
                {
                    primes.Add(ii);
                }

            }

            return primes;
        }

        /// <summary>
        /// Generates a list of all prime numbers between the given bounds (inclusive).
        /// </summary>
        /// <param name="lower"></param> lower bound
        /// <param name="upper"></param> upper bound
        /// <returns></returns> prime number list
        public static List<int> GeneratePrimesBetween(int lower, int upper)
        {
            if (lower < 0)
            {
                lower = 0;
            }
            if (upper < lower)
            {
                throw new ArgumentException("Upper bound must be >= lower bound");
            }

            if (upper < 2)
            {
                //Return empty list
                return new List<int>();
            }

            //First generate the primes up to here so we can do our prime check
            List<int> primesSoFar = GeneratePrimesUpTo(lower - 1);
            List<int> primes = new List<int>();

            //Add 2 in case adding previous numbers won't have gotten us any
            if (lower <= 2)
            {
                primes.Add(2);
            }

            for (int ii = lower; ii <= upper; ii++)
            {
                //Check if number is divisible by prev primes OR newly found primes
                bool divisible = primesSoFar.Any(primeNum => ii % primeNum == 0) ||
                                 primes.Any(primeNum => ii % primeNum == 0);

                if (!divisible)
                {
                    primes.Add(ii);
                }
            }

            return primes;
        }

        public static bool IsPrime(int value)
        {
            bool divisible = false;
            for (int ii = 0; ii < value / 2; ii++)
            {
                if (value % ii == 0)
                {
                    divisible = true;
                }
            }

            return !divisible;
        }
    }
}