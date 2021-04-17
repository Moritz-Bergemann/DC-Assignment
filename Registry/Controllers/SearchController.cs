using APIClasses.Registry;
using Registry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
 
namespace Registry.Controllers
{
    public class SearchController : ApiController
    {
        [Route("api/search/")]
        [HttpPost]
        public List<RegistryData> Search(SearchData data)
        {
            return RegistryModel.Instance.Search(data.Query);
        }

        [Route("api/all")]
        [HttpGet]
        public List<RegistryData> AllServices()
        {
            return RegistryModel.Instance.All();
        }
    }
}