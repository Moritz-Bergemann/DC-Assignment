using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIClasses.Registry;

namespace Registry.Controllers
{
    public class PublishController : ApiController
    {
        [Route("api/publish")]
        [HttpPost]
        public void Publish(RegistryData data)
        {
            
        }

        [Route("api/unpublish")]
        [HttpPost]
        public void Unpublish(EndpointData data)
        {

        }
    }
}