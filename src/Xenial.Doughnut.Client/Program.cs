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

namespace Xenial.Doughnut.Client
{
    public class XpoWebApiProvider : FunctionDataStore
    {
        public const string TokenPart = "Token";
        public const string DataStoreIdPart = "DataStoreId";
        private const string UrlPart = "Url";
        private const string ControllerPart = "Controller";
        private const string SerializationPart = "Serialization";
        public XpoWebApiProvider(IFunction functionClient, IObjectSerializationService objectSerializationService, AutoCreateOption autoCreateOption) : base(functionClient, objectSerializationService, autoCreateOption)
        {
        }
        public static string GetConnectionString(string Url, string Controller, string Token, string DataStoreId)
        {

            return $"{DataStoreBase.XpoProviderTypeParameterName}={XpoProviderTypeString};{UrlPart}={Url};{ControllerPart}={Controller};{TokenPart}={Token};{DataStoreIdPart}={DataStoreId}";
        }
        public const string XpoProviderTypeString = nameof(XpoWebApiProvider);
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            objectsToDisposeOnDisconnect = null;
            ConnectionStringParser Parser = new ConnectionStringParser(connectionString);
            var Url = Parser.GetPartByName(UrlPart);
            var Controller = Parser.GetPartByName(ControllerPart);
            var Token = Parser.GetPartByName(TokenPart);
            var DataStoreId = Parser.GetPartByName(DataStoreIdPart);
            var Serialization = Parser.GetPartByName(SerializationPart);
            Dictionary<string, string> Headers = new Dictionary<string, string>();
            Headers.Add("Authorization", "Bearer " + Token);
            Headers.Add(DataStoreIdPart, DataStoreId);
            Uri uri = new Uri(new Uri(Url), Controller);
            string url = uri.ToString();
            ISerializationAdapter Adapter = null;

            if (Serialization == "NewtonsoftSerializationAdapter")
            {
                Adapter = new NewtonsoftSerializationAdapter();
            }
            if (Serialization == "ProtobufSerializationAdapter")
            {
                Adapter = new ProtobufSerializationAdapter();
            }
            //TODO remove this line when we got an answer from https://github.com/MelbourneDeveloper/RestClient.Net/issues/75
            Adapter = new NewtonsoftSerializationAdapter();

            ApiFunction restClientNetFunctionClient = new ApiFunction(url, Adapter, Headers);

            return new XpoWebApiProvider(restClientNetFunctionClient, new CompressXmlObjectSerializationService(), autoCreateOption);
            //return new AsyncDataStoreWrapper(new XpoWebApiProvider(restClientNetFunctionClient, new SimpleObjectSerializationService(), autoCreateOption));
        }
        public static void Register()
        {
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);

        }
    }
    public class Program
    {
        public static async Task Main(string[] args)
        {
            XpoWebApiProvider.Register();
            var XpoWebApiAspNet = XpoWebApiProvider.GetConnectionString("https://localhost:6001", "/api/XpoWebApi", string.Empty, "001");

            XpoInitializer xpoInitializer = new XpoInitializer(XpoWebApiAspNet, typeof(Invoice), typeof(Customer));

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(_ => xpoInitializer);

            await builder.Build().RunAsync();
        }
    }
}
