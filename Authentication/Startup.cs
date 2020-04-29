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

            #region ͬʱ���� Client_Credentials �� Resource_Owner_Password ģʽ������ͨ�� - OK��
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
                    Description = "��֤���� API",
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
                //��ʾ�Կ�������ע��   
                c.IncludeXmlComments(xmlPath, true);

                //���header��֤��Ϣ
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
                });//���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������Ҫһ�£�������Bearer��
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer ",
                    BearerFormat = "JWT",
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) �����ṹ: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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
            app.UseCors("_myAllowSpecificOrigins"); //���������app.UseRouting()��app.UseAuthorization()֮�䣬����ᱨ��
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
