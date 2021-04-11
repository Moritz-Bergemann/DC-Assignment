using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProvider.Models
{
    public class RegistryException : Exception
    {
        public RegistryException()
        {
        }

        public RegistryException(string message) : base(message)
        {
        }
    }
}