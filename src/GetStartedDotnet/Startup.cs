﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GetStartedDotnet.Models;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GetStartedDotnet.Domain.Interfaces;
using GetStartedDotnet.Domain.Services;
using GetStartedDotnet.Infrastructure;

public class Startup
{
    public IConfigurationRoot Configuration { get; set; }
    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddJsonFile("vcap-local.json", optional: true) // when running locally, store VCAP_SERVICES credentials in vcap-local.json
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        string vcapServices = Environment.GetEnvironmentVariable("VCAP_SERVICES");
        if (vcapServices != null)
        {
            dynamic json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(vcapServices);

            // CF 'cloudantNoSQLDB' service
            if (json.ContainsKey("cloudantNoSQLDB"))
            {
                try
                {
                    Configuration["cloudantNoSQLDB:0:credentials:username"] = json["cloudantNoSQLDB"][0].credentials.username;
                    Console.WriteLine("username ");
                    Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:username"]);
                    Configuration["cloudantNoSQLDB:0:credentials:password"] = json["cloudantNoSQLDB"][0].credentials.password;
                    Console.WriteLine("password ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.password);
                    Configuration["cloudantNoSQLDB:0:credentials:host"] = json["cloudantNoSQLDB"][0].credentials.host;
                    Console.WriteLine("host ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.host);
                    Configuration["cloudantNoSQLDB:0:credentials:url"] = json["cloudantNoSQLDB"][0].credentials.url;
                    Console.WriteLine("url ");
                    Console.WriteLine(json["cloudantNoSQLDB"][0].credentials.url);
                }
                catch (Exception)
                {
                    // Failed to read Cloudant uri, ignore this and continue without a database
                }
            }
            // user-provided service with 'cloudant' in the name
            else if (json.ContainsKey("user-provided"))
            {
                foreach (var service in json["user-provided"])
                {
                    if (((String)service.name).Contains("cloudant"))
                    {
                        try
                        {
                            Configuration["cloudantNoSQLDB:0:credentials:username"] = json["cloudantNoSQLDB"][0].credentials.username;
                            Configuration["cloudantNoSQLDB:0:credentials:password"] = json["cloudantNoSQLDB"][0].credentials.password;
                            Configuration["cloudantNoSQLDB:0:credentials:host"] = json["cloudantNoSQLDB"][0].credentials.host;
                            Configuration["cloudantNoSQLDB:0:credentials:url"] = json["cloudantNoSQLDB"][0].credentials.url;
                        }
                        catch (Exception)
                        {
                            // Failed to read Cloudant uri, ignore this and continue without a database
                        }
                    }
                }
            }

        }
        else if (Configuration.GetSection("services").Exists())
        {
            try
            {
                Configuration["cloudantNoSQLDB:0:credentials:username"] = Configuration["services:cloudantNoSQLDB:0:credentials:username"];
                Console.WriteLine("username ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:username"]);
                Configuration["cloudantNoSQLDB:0:credentials:password"] = Configuration["services:cloudantNoSQLDB:0:credentials:password"];
                Console.WriteLine("password ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:password"]);
                Configuration["cloudantNoSQLDB:0:credentials:host"] = Configuration["services:cloudantNoSQLDB:0:credentials:host"];
                Console.WriteLine("host ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:host"]);
                Configuration["cloudantNoSQLDB:0:credentials:url"] = Configuration["services:cloudantNoSQLDB:0:credentials:url"];
                Console.WriteLine("url ");
                Console.WriteLine(Configuration["cloudantNoSQLDB:0:credentials:url"]);
            }
            catch (Exception)
            {
                // Failed to read Cloudant uri, ignore this and continue without a database
            }

        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.

        services.AddAuthorization();
           
        services.AddScoped<IDB>(db => new DB(
            Configuration["cloudantNoSQLDB:0:credentials:username"],
            Configuration["cloudantNoSQLDB:0:credentials:password"],
            Configuration["cloudantNoSQLDB:0:credentials:host"],
            Configuration["cloudantNoSQLDB:0:credentials:url"],
            "users")
        );

        services.AddTransient<IServices, Services>();
        services.AddTransient<IRepository, Repository>();
            
        services.AddTransient<LoggingHandler>();

        services.AddCors();

        services.AddMvc();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        //var cloudantService = ((ICloudantService)app.ApplicationServices.GetService(typeof(ICloudantService)));

        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });


        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        
    }

    class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("{0}\t{1}", request.Method, request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine(response.StatusCode);
            return response;
        }
    }
}