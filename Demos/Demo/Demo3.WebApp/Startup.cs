﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Demo3.WebApp.Controllers;

namespace Demo3.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataRepository, TotallyRealRepository>();
            services.AddTransient<IApiKeyValidator, TotallRealKeyValidator>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }

    public class TotallyRealRepository : IDataRepository
    {
        public List<ContactInfo> AllContacts = new List<ContactInfo>
            {
                new ContactInfo
                {
                    Name = "First Contact",
                    Email = "first@example.com",
                    Phone = "(555) 123-4567"
                },
                new ContactInfo
                {
                    Name = "Second Contact",
                    Email = "second@example.com",
                    Phone = "(555) 123-4567"
                }
            };

        public IEnumerable<ContactInfo> GetAllContacts()
        {
            return AllContacts;
        }
    }
    public class TotallRealKeyValidator : IApiKeyValidator
    {
        public bool IsApiKeyValid(string apiKey)
        {
            return (apiKey != null);
        }
    }
}
