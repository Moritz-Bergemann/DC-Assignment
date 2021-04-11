using APIClasses.Registry;
using Registry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
 
namespace Registry.Controllers
{
    public class SearchController : ApiController
    {
        [Route("api/Search/{query}")]
        [HttpGet]
        public List<RegistryData> Search(string query)
        {
            return RegistryModel.Instance.Search(query);
        }

        [Route("api/all")]
        [HttpGet]
        public List<RegistryData> AllServices()
        {
            return RegistryModel.Instance.All();
        }
    }
}