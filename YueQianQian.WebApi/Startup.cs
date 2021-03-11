using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using YueQianQian.Common;
using AutoMapper;
using YueQianQian.Common.Auth;
using Microsoft.AspNetCore.Http;
using YueQianQian.Repository;
using FreeSql;
using YueQianQian.Common.Attributes;
using YueQianQian.Common.Aop;
using NLog.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace YueQianQian.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域设置
            services.AddCors(c =>
            {
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    // .WithOrigins(AppSettingsHelper.Configuration["Startup:AllowOrigins"].Split('|'))
                    .AllowAnyOrigin()  //支持所有跨域
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion
            #region swagger说明文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{AppSettingsHelper.Configuration["Startup:ApiName"]} 接口文档",
                    Description = $"{AppSettingsHelper.Configuration["Startup:ApiName"]} HTTP API ",
                    Version = "v1"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"}
                           },new string[] { }
                        }
                    });
            });
            #endregion
            #region 配置Json格式
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                // 忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 不使用驼峰
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // 如字段为null值，该字段不会返回到前端
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            #endregion
            #region JWT鉴权授权
            //1.Nuget引入程序包：Microsoft.AspNetCore.Authentication.JwtBearer 
            //services.AddAuthentication();//禁用  
            var validAudience = AppSettingsHelper.Configuration["JwtSetting:Audience"];
            var validIssuer = AppSettingsHelper.Configuration["JwtSetting:Issuer"];
            var securityKey = AppSettingsHelper.Configuration["JwtSetting:SecurityKey"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //默认授权机制名称；                                      
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,//是否验证Issuer
                             ValidateAudience = true,//是否验证Audience
                             ValidateLifetime = false,//是否验证失效时间
                             ValidateIssuerSigningKey = true,//是否验证SecurityKey
                             ValidAudience = validAudience,//Audience
                             ValidIssuer = validIssuer,//Issuer，这两项和前面签发jwt的设置一致  表示谁签发的Token
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))//拿到SecurityKey
                             //AudienceValidator = (m, n, z) =>
                             //{ 
                             //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                             //},//自定义校验规则，可以新登录后将之前的无效 
                         };

                     });

            #endregion

            #region 用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICurrentUser, CurrentUser>();
            #endregion
            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("YueQianQian.Service");
            services.AddAutoMapper(serviceAssembly);
            #endregion
            #region filter
            services.AddMvc(options =>
            {
                options.Filters.Add<CustomAuthorzationFilter>();
                options.Filters.Add<CustomExceptionFilter>();
                options.Filters.Add<CustomActionFilter>();

            });
            #endregion
            #region freesql
            services.AddSingleton<IFreeSql>(FreeSqlDB.Get(Env));
            services.AddScoped<UnitOfWorkManager>();
            services.AddFreeRepository(null, typeof(Startup).Assembly);
            #endregion
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //请求错误提示配置
            //app.UseErrorHandling();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YueQianQian API V1");
            });
            //日志
            // app.UseMiddleware<NLogMiddleware>();
            app.UseRouting();

            #region 跨域设置
            app.UseCors("LimitRequests");
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region 自动注入服务
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var interceptorServiceTypes = new List<Type>();
            #region Aop
            builder.RegisterType<TransactionInterceptor>();
            interceptorServiceTypes.Add(typeof(TransactionInterceptor));

            #endregion
            #region Repository
            var assemblyRepository = Assembly.Load("YueQianQian.Repository");
            builder.RegisterAssemblyTypes(assemblyRepository)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            #endregion
            #region Service
            var assemblysServices = Assembly.Load("YueQianQian.Service");
            builder.RegisterAssemblyTypes(assemblysServices)
               .InstancePerDependency()//瞬时单例
               .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors() //引用Autofac.Extras.DynamicProxy;
               .InterceptedBy(interceptorServiceTypes.ToArray());
            #endregion
        }
        #endregion

    }
}
