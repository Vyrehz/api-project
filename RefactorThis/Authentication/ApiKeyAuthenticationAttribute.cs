using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace refactor_this.Authentication
{
    public class ApiKeyAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var query = actionContext.Request.Headers.Authorization;
            if (query == null || !CheckApiKey(query.Parameter))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        private static bool CheckApiKey(string apiKey)
        {
            return apiKey == "test_api_key";
        }
    }
}