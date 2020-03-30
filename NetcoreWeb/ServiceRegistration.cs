using Microsoft.Extensions.DependencyInjection;
using ShoppingApi.Stores;
using ShoppingApi.Stores.Interface;
using ShoppingApi.Managers;
namespace ShoppingApi
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public class ServiceRegistration
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        public static void Start(IServiceCollection services)
        {
            services.AddScoped<CustomerManager>();
            services.AddScoped<ICustomerStore, CustomerStore>();
        }


    }
}
