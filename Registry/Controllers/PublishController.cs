using APIClasses.Registry;
using APIClasses.Security;
using Registry.Models;
using ServiceProvider.Models;
using System.Web.Http;

namespace Registry.Controllers
{
    public class PublishController : ApiController
    {
        [Route("api/publish")]
        [HttpPost]
        public PublishResult Publish(PublishRequest request)
        {
            try
            {
                //Do authentication check first
                if (RegistryModel.Instance.TestAuthentication(request.Token))
                {
                    ServiceData data = request.Data;

                    PublishResult result = new PublishResult();
                    result.Status = "Accepted";

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
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }
            } catch (AuthenticationException)
            {
                return new PublishResult(false, "Authentication Error", false, null);
            }
        }

        [Route("api/unpublish")]
        [HttpPost]
        public PublishResult Unpublish(UnpublishRequest request)
        {
            try
            {
                if (RegistryModel.Instance.TestAuthentication(request.Token))
                {
                    bool found = RegistryModel.Instance.Unpublish(request);

                    return new PublishResult
                    {
                        Status = "Accepted",
                        Success = found,
                        Message = found
                            ? "Service unpublished successfully"
                            : "Service with given API endpoint was not found in registry"
                    };
                }
                else
                {
                    throw new AuthenticationException("Authentication denied");
                }
            }
            catch (AuthenticationException)
            {
                return new PublishResult(false, "Authentication Error", false, null);
            }
        }
    }
}