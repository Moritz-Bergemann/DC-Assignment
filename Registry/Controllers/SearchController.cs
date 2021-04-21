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
        public SearchResponse Search(SearchRequest request)
        {
            try
            {
                //Check the token on the registry server
                if (RegistryModel.Instance.TestAuthentication(request.Token))
                {
                    List<ServiceData> searchResult = RegistryModel.Instance.Search(request.Query);

                    return new SearchResponse(true, null, searchResult);
                }
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }
            }
            catch (AuthenticationException)
            {
                return new SearchResponse(false, "Authentication Error", null);
            }
        }

        [Route("api/all")]
        [HttpPost]
        public SearchResponse AllServices(SecureRequest request)
        {
            try
            {
                if (RegistryModel.Instance.TestAuthentication(request.Token))
                {
                    List<ServiceData> searchResult = RegistryModel.Instance.All();

                    return new SearchResponse(true, null, searchResult);
                }
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }

            }
            catch (AuthenticationException)
            {
                return new SearchResponse(false, "Authentication Error", null);
            }

        }
    }
}