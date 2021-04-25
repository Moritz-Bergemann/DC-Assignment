using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ClientGUI
{
    /// <summary>
    /// Static class containing functions to make library-compatible asynchronous REST requests.
    /// </summary>
    public class AsyncRest
    {
        public static async Task<IRestResponse> AsyncGet(RestRequest request, RestClient client)
        {
            return await Task.Run(() => client.Get(request));
        }

        public static async Task<IRestResponse> AsyncPost(RestRequest request, RestClient client)
        {
            return await Task.Run(() => client.Post(request));
        }
    }
}
