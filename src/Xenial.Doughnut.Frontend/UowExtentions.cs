using System;
using System.Net.Http;

using BIT.Xpo;

using DevExpress.Xpo.DB;

using Microsoft.Extensions.DependencyInjection;

using Xenial.Doughnut.Model;

namespace Xenial.Doughnut.Frontend
{
    public static class UowExtentions
    {
        public static IServiceCollection AddHttpUnitOfWork(this IServiceCollection services)
        {
            XpoInitializer xpoInitializer = null;

            services.AddScoped(s =>
            {
                DataStoreBase.RegisterDataStoreProvider(XpoWebApiHttpProvider.XpoProviderTypeString, CreateProviderFromString);

                IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
                {
                    var clientFactory = s.GetService<IHttpClientFactory>();
                    return XpoWebApiHttpProvider.CreateProviderFromString(clientFactory.CreateClient("Xenial.Doughnut.Backend"), connectionString, autoCreateOption, out objectsToDisposeOnDisconnect);
                }

                if (xpoInitializer == null)
                {
                    var XpoWebApiAspNet = XpoWebApiHttpProvider.GetConnectionString("https://localhost:7001", "/api/XpoWebApi", string.Empty, "001");

                    xpoInitializer = new XpoInitializer(XpoWebApiAspNet, ModelTypeList.ModelTypes);
                }

                return xpoInitializer.CreateUnitOfWork();
            });

            return services;
        }
    }
}
