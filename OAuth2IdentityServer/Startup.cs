using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OAuth2IdentityServer.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.PlatformAbstractions;

namespace OAuth2IdentityServer
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
            services.AddControllers();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.1.0",
                    Title = "Ray WebAPI",
                    Description = "��ܼ���",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "RayWang", Email = "2271272653@qq.com", Url = "http://www.cnblogs.com/RayWang" }
                });
                //���ע�ͷ���
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var apiXmlPath = Path.Combine(basePath, "APIHelp.xml");
                var entityXmlPath = Path.Combine(basePath, "EntityHelp.xml");
                c.IncludeXmlComments(apiXmlPath, true);//��������ע�ͣ�true��ʾ��ʾ������ע�ͣ�
                c.IncludeXmlComments(entityXmlPath);

                //��ӿ�����ע��
                //c.DocumentFilter<SwaggerDocTag>();

                //���header��֤��Ϣ
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������Ҫһ�£�������Bearer��
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) �����ṹ: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = "apiKey"
                });
            });
            #endregion

            #region ��֤
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    JwtAuthConf igModel jwtConfig = new JwtAuthConfigModel();
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "RayPI",
                        ValidAudience = "wr",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.JWTSecretKey)),

                        /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
                        RequireSignedTokens = true,
                        // SaveSigninToken = false,
                        // ValidateActor = false,
                        // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        // �Ƿ�Ҫ��Token��Claims�б������ Expires
                        RequireExpirationTime = true,
                        // ����ķ�����ʱ��ƫ����
                        // ClockSkew = TimeSpan.FromSeconds(300),
                        // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                        ValidateLifetime = true
                    };
                });
            #endregion

            #region ��Ȩ
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClient", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("RequireAdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion

            #region CORS
            services.AddCors(c =>
            {
                c.AddPolicy("Any", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });

                c.AddPolicy("Limit", policy =>
                {
                    policy
                    .WithOrigins("localhost:8083")
                    .WithMethods("get", "post", "put", "delete")
                    //.WithHeaders("Authorization");
                    .AllowAnyHeader();
                });
            });
            #endregion
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5001";
                options.RequireHttpsMetadata = false;
                options.Audience = "api1";
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region ��Ӧ grant_type = Client_Credentials ģʽ������ͨ�� - OK��

            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryApiResources(Config.GetResources())
            //    .AddInMemoryClients(Clients.GetClients());

            #endregion

            #region ��Ӧ grant_type = Resource_Owner_Password ģʽ������ͨ�� - OK��

            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryApiResources(Config.GetResources())
            //    .AddInMemoryClients(Clients.GetClients())
            //    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            //    .AddProfileService<ProfileService>();

            #endregion

            #region ͬʱ���� Client_Credentials �� Resource_Owner_Password ģʽ������ͨ�� - OK��
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetResources())
                .AddInMemoryClients(Clients.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication(); //��֤
            app.UseAuthorization();//��Ȩ
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
