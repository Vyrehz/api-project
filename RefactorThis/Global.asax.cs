using System.Web.Http;
using Unity;

namespace refactor_this
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = new UnityContainer();

            UnityConfig.RegisterComponents(container);
        }
    }
}
