using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sisc.Api.Common.Runtime.Caching;
using Sisc.RestApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;

namespace Sisc.RestApi
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
            var listConnectionStrings = new List<string>
            {
                Configuration.GetConnectionString("SISC_DB_common")
            };

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IMockCreator, MockCreator>();
            services.AddSingleton<ICacheHandler, CacheHandler>();
            services.AddApiVersioning();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SISC API",
                    Description = "ASP.NET Core Web API for SISC",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Patrick Imboden",
                        Email = "inkognito2010@live.com" ,
                        Url = "https://github.com/pimboden"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var apiLibStartup = new Api.Lib.Startup();
            apiLibStartup.ConfigureServices(services,listConnectionStrings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                loggerFactory.AddLog4Net(Configuration.GetValue<string>("Log4NetConfigFile:Name"));
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SISC API V1");
                    c.RoutePrefix = "docs";
                });
                app.UseMvc();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Log(1, "some", exception.Message);
            }
        }
    }
}
