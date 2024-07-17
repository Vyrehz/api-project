using System.Web.Http;
using Unity.AspNet.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(refactor_this.UnityWebApiActivator), nameof(refactor_this.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(refactor_this.UnityWebApiActivator), nameof(refactor_this.UnityWebApiActivator.Shutdown))]

namespace refactor_this
{
    public static class UnityWebApiActivator
    {
        public static void Start() 
        {
            var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}