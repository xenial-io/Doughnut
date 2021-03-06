﻿using System;
using System.Collections.Generic;
using System.Net.Http;

using BIT.Data.DataTransfer;
using BIT.Data.Services;

using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;

namespace Xenial.Doughnut.Frontend
{
    public class XpoWebApiHttpProvider : FunctionDataStoreAsync
    {
        public const string TokenPart = "Token";
        public const string DataStoreIdPart = "DataStoreId";
        private const string urlPart = "Url";
        private const string controllerPart = "Controller";
        public XpoWebApiHttpProvider(
            IFunction functionClient,
            IObjectSerializationService objectSerializationService,
            AutoCreateOption autoCreateOption
        ) : base(functionClient, objectSerializationService, autoCreateOption) { }

        public static string GetConnectionString(
            string url,
            string controller,
            string token,
            string dataStoreId
        ) => $"{DataStoreBase.XpoProviderTypeParameterName}={XpoProviderTypeString};{urlPart}={url};{controllerPart}={controller};{TokenPart}={token};{DataStoreIdPart}={dataStoreId}";

        public const string XpoProviderTypeString = nameof(XpoWebApiHttpProvider);

        public static IDataStore CreateProviderFromString(HttpClient client, string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            objectsToDisposeOnDisconnect = null;
            var Parser = new ConnectionStringParser(connectionString);
            var Url = Parser.GetPartByName(urlPart);
            var Controller = Parser.GetPartByName(controllerPart);
            var DataStoreId = Parser.GetPartByName(DataStoreIdPart);

            var Headers = new Dictionary<string, string>
            {
                { DataStoreIdPart, DataStoreId }
            };
            var uri = new Uri(new Uri(Url), Controller);
            var url = uri.ToString();

            var restClientNetFunctionClient = new HttpClientFunction(client, url, Headers);

            return new XpoWebApiHttpProvider(restClientNetFunctionClient, new SimpleObjectSerializationService(), autoCreateOption);
        }

    }
}
