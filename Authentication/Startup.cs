using Authentication.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Authentication.Stores;
using System.IO;

namespace Authentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            #region Mysql                    
            services.AddDbContext<AuthenticationDbContext>(options =>
     options.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            #endregion

            #region 同时兼容 Client_Credentials 和 Resource_Owner_Password 模式（测试通过 - OK）
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryApiResources(Config.GetResources())
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryIdentityResources(Config.GetIdentityResourceResources())
                //.AddTestUsers(Config.TestUsers)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Authentiction Api",
                    Description = "认证中心 API",
                    //TermsOfService = new Uri("api/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Authentiction Api",
                        Email = string.Empty,
                        Url = new Uri("http://www.topimage.design/")
                    }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "Authentication.xml");
                //显示对控制器的注释   
                c.IncludeXmlComments(xmlPath, true);

                //添加header验证信息
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                           {
                               new OpenApiSecurityScheme
                               {
                                   Reference = new OpenApiReference {
                                       Type = ReferenceType.SecurityScheme,
                                       Id = "Bearer"
                                   }
                               },
                               new string[] { }
                           }
                });//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer ",
                    BearerFormat = "JWT",
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion
            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins", corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");

                });
            });
            services.AddScoped<ResourceOwnerPasswordValidator>();
            services.AddScoped<IUserStore, UserStore>();
            //// demo versions
            //services.AddTransient<IRedirectUriValidator, DemoRedirectValidator>();
            //services.AddTransient<ICorsPolicyService, DemoCorsPolicy>();
            services.AddControllers().AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseHttpsRedirection();

            /// app.UseStaticFiles();


            app.UseRouting();
            /// app.UseCookiePolicy();
            app.UseCors("_myAllowSpecificOrigins"); //此项必须在app.UseRouting()和app.UseAuthorization()之间，否则会报错。
            app.UseAuthorization();
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            #endregion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
