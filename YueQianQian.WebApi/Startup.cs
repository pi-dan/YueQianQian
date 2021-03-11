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
            #region ��������
            services.AddCors(c =>
            {
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    // .WithOrigins(AppSettingsHelper.Configuration["Startup:AllowOrigins"].Split('|'))
                    .AllowAnyOrigin()  //֧�����п���
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion
            #region swagger˵���ĵ�
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{AppSettingsHelper.Configuration["Startup:ApiName"]} �ӿ��ĵ�",
                    Description = $"{AppSettingsHelper.Configuration["Startup:ApiName"]} HTTP API ",
                    Version = "v1"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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
            #region ����Json��ʽ
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                // ����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // ��ʹ���շ�
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // ����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            #endregion
            #region JWT��Ȩ��Ȩ
            //1.Nuget����������Microsoft.AspNetCore.Authentication.JwtBearer 
            //services.AddAuthentication();//����  
            var validAudience = AppSettingsHelper.Configuration["JwtSetting:Audience"];
            var validIssuer = AppSettingsHelper.Configuration["JwtSetting:Issuer"];
            var securityKey = AppSettingsHelper.Configuration["JwtSetting:SecurityKey"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //Ĭ����Ȩ�������ƣ�                                      
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,//�Ƿ���֤Issuer
                             ValidateAudience = true,//�Ƿ���֤Audience
                             ValidateLifetime = false,//�Ƿ���֤ʧЧʱ��
                             ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                             ValidAudience = validAudience,//Audience
                             ValidIssuer = validIssuer,//Issuer���������ǰ��ǩ��jwt������һ��  ��ʾ˭ǩ����Token
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))//�õ�SecurityKey
                             //AudienceValidator = (m, n, z) =>
                             //{ 
                             //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                             //},//�Զ���У����򣬿����µ�¼��֮ǰ����Ч 
                         };

                     });

            #endregion

            #region �û���Ϣ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICurrentUser, CurrentUser>();
            #endregion
            #region AutoMapper �Զ�ӳ��
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

            //���������ʾ����
            //app.UseErrorHandling();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YueQianQian API V1");
            });
            //��־
            // app.UseMiddleware<NLogMiddleware>();
            app.UseRouting();

            #region ��������
            app.UseCors("LimitRequests");
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region �Զ�ע�����
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
               .InstancePerDependency()//˲ʱ����
               .AsImplementedInterfaces()////�Զ�����ʵ�ֵ����нӿ����ͱ�¶������IDisposable�ӿڣ�
               .EnableInterfaceInterceptors() //����Autofac.Extras.DynamicProxy;
               .InterceptedBy(interceptorServiceTypes.ToArray());
            #endregion
        }
        #endregion

    }
}
