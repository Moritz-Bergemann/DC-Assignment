using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses.Security
{
    /// <summary>
    /// Struct representing request to secure server, includes token 
    /// </summary>
    class SecureRequest
    {
        public int Token;

        public SecureRequest(int token)
        {
            Token = token;
        }
    }

    /// <summary>
    /// Struct class representing response from secure application (if secure server access was successful, and a reason if it was not).
    /// Status can be one of 'Accepted' or 'Denied'. If status is 'Accepted', token will be 
    /// </summary>
    class SecureResponse
    {
        public string Status;
        public string Reason;

        public SecureResponse(bool accepted, string reason)
        {
            Status = accepted ? "Accepted" : "Denied";
            Reason = reason;
        }
    }
}
