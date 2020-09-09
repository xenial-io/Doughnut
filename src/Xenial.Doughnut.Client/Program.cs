using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BIT.Xpo.Providers.WebApi.Client;
using BIT.Xpo;
using DevExpress.Xpo.DB;
using BIT.Data.Functions.RestClientNet;
using RestClient.Net.Abstractions;
using DevExpress.Xpo.DB.Helpers;
using BIT.Data.Functions;
using BIT.Xpo.DataStores;
using BIT.Data.Services;
using Xenial.Doughnut.Model;
using System.Threading;

namespace Xenial.Doughnut.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DevExpress.Xpo.SimpleDataLayer.SuppressReentrancyAndThreadSafetyCheck = true;

            XpoWebApiHttpProvider.Register();

            var XpoWebApiAspNet = XpoWebApiHttpProvider.GetConnectionString("https://localhost:6001", "/api/XpoWebApi", string.Empty, "001");

            var xpoInitializer = new XpoInitializer(XpoWebApiAspNet, ModelTypeList.ModelTypes);
            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(_ => xpoInitializer);

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
