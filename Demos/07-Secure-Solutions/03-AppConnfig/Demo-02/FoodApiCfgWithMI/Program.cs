using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodApi {
    public class Program {

        public static void Main (string[] args) {
            CreateWebHostBuilder (args).Build ().Run ();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var settings = config.Build();
            var credentials = new ManagedIdentityCredential();
            var ep = settings["AppConfig:Endpoint"];

            config.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(ep), credentials)
                        .ConfigureKeyVault(kv =>
                        {
                            kv.SetCredential(credentials);
                        });
            });
        })
        .UseStartup<Startup>();
    }
}