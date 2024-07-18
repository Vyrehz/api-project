using System;
using System.Web.Http;
using refactor_this.Repositories;
using refactor_this.Services;
using Unity;
using Unity.AspNet.WebApi;

namespace refactor_this
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterComponents(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;

        public static void RegisterComponents(IUnityContainer container)
        {
            // Register interface mappings
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductOptionService, ProductOptionService>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductOptionRepository, ProductOptionRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}