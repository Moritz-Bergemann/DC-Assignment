using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIClasses.Registry;
using Registry.Models;
using ServiceProvider.Models;

namespace Registry.Controllers
{
    public class PublishController : ApiController
    {
        [Route("api/publish")]
        [HttpPost]
        public PublishResult Publish(RegistryData data)
        {
            PublishResult result = new PublishResult();

            if (data == null)
            {
                result.Success = false;
                result.Message = "Could not publish service: Empty input";
                return result;
            }

            try
            {
                RegistryModel.Instance.Publish(data);
                result.Success = true;
                result.Message = "Service published successfully";
            }
            catch (RegistryException r)
            {
                result.Success = false;
                result.Message = $"Could not publish service: {r.Message}";
            }

            return result;
        }

        [Route("api/unpublish")]
        [HttpPost]
        public PublishResult Unpublish(EndpointData endpointData)
        {
            bool found = RegistryModel.Instance.Unpublish(endpointData);

            return new PublishResult
            {
                Success = found,
                Message = found
                    ? "Service unpublished successfully"
                    : "Service with given API endpoint was not found in registry"
            };
        }
    }
}