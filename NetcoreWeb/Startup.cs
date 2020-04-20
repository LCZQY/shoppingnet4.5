using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShoppingApi.Models;
using System;
using System.IO;
using ZapiCore;

namespace ShoppingApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//名字随便起
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region AutoMapper
            services.AddAutoMapper(typeof(ServiceProfile));  //ServiceProfile为你Mapper的类
            #endregion      
            #region CORS 不可以同时打开AllowAnyOrigin，AllowAnyMethod，AllowAnyHeader，AllowCredentials 这个东西搞了太久了 （一定要设置 SetIsOriginAllowedToAllowWildcardSubdomains)


            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");

                });
            });
            //以下是错误的
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowAnyOrigin", policy =>
            //    {
            //        policy.AllowAnyOrigin()//允许任何源
            //        .AllowAnyMethod()//允许任何方式
            //        .AllowAnyHeader()//允许任何头
            //        .AllowCredentials();//允许cookie
            //    });
            //});
            #endregion
            #region Mysql                    
            services.AddDbContext<ShoppingDbContext>(options =>
     options.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ShoppingSystem Api",
                    Description = "后台管理系统API",
                    //TermsOfService = new Uri("api/"),
                    Contact = new OpenApiContact
                    {
                        Name = "ShoppingSystem Api",
                        Email = string.Empty,
                        Url = new Uri("http://www.topimage.design/")
                    }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "ShoppingApi.xml");
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

            #region 认证
      
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001"; //认证中心地址
                options.RequireHttpsMetadata = false;
                options.Audience = "api1"; //保护资源标识      
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5); //5分钟检查一下TOken的有效性
                options.TokenValidationParameters.RequireExpirationTime = true; //接收到的Token 必须是带有超时时间的
            });
            #endregion
            #region 授权 


            #endregion
            //服务注册
            ServiceRegistration.Start(services);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 加入中间件异常处理
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
            #endregion
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins); //此项必须在app.UseRouting()和app.UseAuthorization()之间，否则会报错。
            app.UseHttpsRedirection();

            app.UseAuthentication(); //使用认证
            app.UseAuthorization(); //使用授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
            });

        }
    }

}