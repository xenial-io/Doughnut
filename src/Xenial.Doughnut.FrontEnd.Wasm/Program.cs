﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

using BIT.Xpo;

using DevExpress.Xpo.DB;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Skclusive.Material.Component;

using Xenial.Doughnut.Model;

namespace Xenial.Doughnut.FrontEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DevExpress.Xpo.SimpleDataLayer.SuppressReentrancyAndThreadSafetyCheck = true;

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.TryAddMaterialServices(new MaterialConfigBuilder().Build());

            builder.Services.TryAddDashboardViewServices
            (
                new DashboardViewConfigBuilder()
                .WithIsServer(false)
                .WithIsPreRendering(false)
                .WithResponsive(true)
                .Build()
            );

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            builder.Services.AddHttpClient("Xenial.Doughnut.Server")
                 .AddHttpMessageHandler(sp =>
                 {
                     var handler = sp.GetService<AuthorizationMessageHandler>()
                         .ConfigureHandler(
                             authorizedUrls: new[] { "https://localhost:7001" },
                             scopes: new[]
                             {
                                 "Xenial.Doughnut.Server"
                             });
                     return handler;
                 });

            XpoInitializer xpoInitializer = null;

            builder.Services.AddScoped(s =>
            {
                DataStoreBase.RegisterDataStoreProvider(XpoWebApiHttpProvider.XpoProviderTypeString, CreateProviderFromString);

                IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
                {
                    var clientFactory = s.GetService<IHttpClientFactory>();
                    return XpoWebApiHttpProvider.CreateProviderFromString(clientFactory.CreateClient("Xenial.Doughnut.Server"), connectionString, autoCreateOption, out objectsToDisposeOnDisconnect);
                }

                if (xpoInitializer == null)
                {
                    var XpoWebApiAspNet = XpoWebApiHttpProvider.GetConnectionString("https://localhost:7001", "/api/XpoWebApi", string.Empty, "001");

                    xpoInitializer = new XpoInitializer(XpoWebApiAspNet, ModelTypeList.ModelTypes);
                }

                return xpoInitializer.CreateUnitOfWork();
            });

            builder.Services.AddApiAuthorization(options =>
            {
                options.AuthenticationPaths.LogOutSucceededPath = "";
            });

            await builder.Build().RunAsync();
        }
    }
}
