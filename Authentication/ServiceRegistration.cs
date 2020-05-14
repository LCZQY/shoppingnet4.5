using Authentication.Managers;
using Authentication.Models;
using Authentication.Stores;
using Microsoft.Extensions.DependencyInjection;
using ZapiCore;

namespace Authentication
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
            services.AddScoped<ITransaction<AuthenticationDbContext>, Transaction<AuthenticationDbContext>>();
            services.AddScoped<ResourceOwnerPasswordValidator>();

            services.AddScoped<UserManager>();
            services.AddScoped<IUserStore, UserStore>();
            services.AddTransient<IUserRoleStore, UserRoleStore>();
            services.AddScoped<User_Role>();


            services.AddScoped<RoleManager>();
            services.AddScoped<IRoleStore, RoleStore>();
            services.AddScoped<IRolePermissionStore, RolePermissionStore>();


            services.AddScoped<PermissionManager>();
            services.AddScoped<IPermissionStore, PermissionitemStore>();

       
        }


    }
}
