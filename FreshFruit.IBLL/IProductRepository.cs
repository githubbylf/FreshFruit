using AutofacContrib.DynamicProxy2;
using FreshFruit.IoC.DI;

namespace FreshFruit.IBLL
{
    [Intercept(typeof(LoggingInterceptor))]
    public interface IProductRepository
    {
        int AddObject(object obj);
    }
}
