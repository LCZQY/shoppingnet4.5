using Microsoft.Extensions.DependencyInjection;
using NetcoreWeb.Stores;
using NetcoreWeb.Stores.Interface;

namespace NetcoreWeb
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
            services.AddScoped<ICustomerStore, CustomerStore>();
        }


    }
}
