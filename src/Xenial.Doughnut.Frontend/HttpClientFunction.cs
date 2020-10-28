using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using BIT.Data.DataTransfer;

namespace Xenial.Doughnut.Frontend
{
    public class HttpClientFunction : IFunction, IFunctionAsync
    {
        public HttpClient Client { get; }
        public string Url { get; }

        /// <summary>
        /// Initializes a new instance of the ApiFunction class that uses your instance of RestClient
        /// </summary>
        /// <param name="client">An instance of RestClientNet</param>
        /// <param name="url">Api Url</param>
        /// <param name="headers">Additional headers for the api request</param>
        public HttpClientFunction(HttpClient client, string url, IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            Client = client;
            Url = url;
        }

        /// <summary>
        /// Executes the funtion asynchronous with the specified parameters
        /// </summary>
        /// <param name="parameters">An implementation of IDataParameters</param>
        /// <returns>A task with the result of the function</returns>
        public async Task<IDataResult> ExecuteFunctionAsync(IDataParameters parameters)
        {
            var result = await Client.PostAsJsonAsync(Url, parameters);
            result.EnsureSuccessStatusCode();
            var body = await result.Content.ReadFromJsonAsync<DataResult>();
            return body;
        }

        /// <summary>
        /// Executes the funtion synchronous with the specified parameters
        /// </summary>
        /// <param name="parameters">An implementation of IDataParameters</param>
        /// <returns>The result of the function</returns>
        public IDataResult ExecuteFunction(IDataParameters parameters)
            => throw new PlatformNotSupportedException();
    }
}
