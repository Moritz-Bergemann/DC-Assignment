using APIClasses.Registry;
using Registry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using APIClasses.Security;

namespace Registry.Controllers
{
    public class SearchController : ApiController
    {
        [Route("api/search/")]
        [HttpPost]
        public SearchResponse Search(SearchData data)
        {
            //Check the token on the registry server
            try
            {
                if (RegistryModel.Instance.TestAuthentication(data.Token))
                {
                    List<RegistryData> searchResult = RegistryModel.Instance.Search(data.Query);

                    return new SearchResponse(true, null, searchResult);
                }
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }
            }
            catch (AuthenticationException)
            {
                return new SearchResponse(false, "Connection to authentication server failed", null);
            }
        }

        [Route("api/all")]
        [HttpGet]
        public SearchResponse AllServices(SecureRequest request)
        {
            try
            {
                if (RegistryModel.Instance.TestAuthentication(request.Token))
                {
                    List<RegistryData> searchResult = RegistryModel.Instance.All();

                    return new SearchResponse(true, null, searchResult);
                }
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }

            }
            catch (AuthenticationException)
            {
                return new SearchResponse(false, "Connection to authentication server failed", null);
            }

        }
    }
}