using Arctic.AppCodes;
using Arctic.AppSeqs;
using Arctic.Books;
using Arctic.EventBus;
using Arctic.NHibernateExtensions;
using Autofac;
using AutofacSerilogIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;

namespace Arctic.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arctic.Web", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Configuration.Bind("JwtSetting", options);
                });
            services.AddSingleton<IAuthorizationPolicyProvider, OperationTypePolicyProvider>();
            services.AddSingleton<IOperaionTypePermissions, DefaultOperaionTypePermissions>();
            services.AddAuthorization(options => {
            });

        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterLogger();

            builder.AddAppCodes();
            builder.AddAppSeqs();
            builder.AddBooks();

            //builder.RegisterType<OperaionTypePermissions>().AsImplementedInterfaces().SingleInstance();
            builder.ConfigureNHibernate(Configuration.GetSection("NHibernate").Get<NHibernateOptions>());
            builder.AddEventBus(Configuration.GetSection("EventBus").Get<SimpleEventBusOptions>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arctic.Web v1"));
            }

            // TODO 改为安全的设置
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHsts();

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
                {
                    await next();
                }
            });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
#warning 仅用于调试
                app.Use(async (context, next) =>
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(ClaimTypes.Role, "dev"),
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    context.User = principal;
                    await next();
                });
            }

            app.Use(async (context, next) =>
            {
                using (LogContext.PushProperty("UserName", context.User.Identity.Name))
                {
                    await next();
                }
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
