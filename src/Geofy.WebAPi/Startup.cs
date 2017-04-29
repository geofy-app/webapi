﻿using System;
using Geofy.WebAPi.Authorization;
using Geofy.WebAPi.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Geofy.WebAPi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json");
            }

            builder.AddEnvironmentVariables();
            _configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        private readonly IConfigurationRoot _configuration;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(
                    options =>
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddAuthentication();
            services.AddCaching();
            return services.BuildContainer(_configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(_configuration.GetSection("Logging"))
                .AddDebug();

            
            app.UseJwtBearerAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
                options.RequireHttpsMetadata = false;

                options.Audience = "http://localhost:5000/";
                options.Authority = "http://localhost:5000/";
            }).UseOpenIdConnectServer(options =>
            {
                options.Provider = new AuthorizationServerProvider();
                options.AllowInsecureHttp = true;
                options.TokenEndpointPath = "/token";

                options.AccessTokenLifetime = TimeSpan.FromMinutes(20);
                options.RefreshTokenLifetime = TimeSpan.FromHours(24);
            }).UseMvc();
        }



        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
