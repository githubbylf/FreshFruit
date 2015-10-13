
using Autofac;
using Autofac.Integration.Mvc;
using FreshFruit.BLL;
using FreshFruit.IBLL;
using System.Web.Mvc;

namespace FreshFruit.WebSites.Official
{
    public class IocConfig
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<ProductRepository>().As<IProductRepository>();

            //autofac register dependencies
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
