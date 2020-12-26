// Copyright 2020 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Arctic.AppCodes;
using Arctic.AppSeqs;
using Arctic.AspNetCore;
using Arctic.Books;
using Arctic.EventBus;
using Arctic.NHibernateExtensions;
using Autofac;
using AutofacSerilogIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Security.Principal;

#pragma warning disable 1591

namespace Arctic.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutofacContainer = default!;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arctic.Web", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHttpContextAccessor();
            services.AddTransient<IPrincipal>(provider => 
                provider.GetService<IHttpContextAccessor>()?.HttpContext?.User!
                );


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Configuration.Bind("JwtSetting", options);
                });
            services.AddOperationType();
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
            builder.AddEventBus(Configuration.GetSection("EventBus").Get<SimpleEventBusOptions>());
            builder.AddNHibernate();
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
#warning 仅用于调试期间
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
                using (LogContext.PushProperty("UserName", context.User.Identity?.Name ?? "-"))
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

#pragma warning restore 1591
