using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using APIClasses;
using ServerInterfaceLib;

namespace ServiceProvider.Models
{
    public class MathModel
    {
        private static string authUrl = NetworkPaths.AuthenticatorUrl;
        private IAuthenticationServer _authServer;

        public static MathModel Instance
        {
            get;
        } = new MathModel();

        public MathModel()
        {
            //Create connection factory for connection to auth server
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, authUrl);
            _authServer = serverChannelFactory.CreateChannel();
        }

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
            //Validity check
            if (upper < lower)
            {
                throw new ArgumentException("Upper bound must be >= lower bound");
            }

            //If upper is below first prime number, return nothing
            if (upper < 2)
            {
                //Return empty list
                return new List<int>();
            }

            //If lower is below first prime number, set it to first prime number
            if (lower < 2)
            {
                lower = 2;
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
            if (value < 2)
            {
                return false;
            }

            bool divisible = false;
            for (int ii = 2; ii < value / 2 + 1; ii++)
            {
                if (value % ii == 0)
                {
                    divisible = true;
                }
            }

            return !divisible;
        }

        /// <summary>
        /// Returns true if token could be successfully authenticated with authentication server, and false otherwise (including in case of server error).
        /// </summary>
        /// <param name="token">Authentication token to test against</param>
        /// <returns></returns>
        public bool CheckAuthentication(int token)
        {
            bool result;
            try
            {
                string validationResult = _authServer.Validate(token);

                result = validationResult.Equals("validated");
            }
            catch (CommunicationException)
            {
                result = false;
            }

            return result;
        }
    }
}