using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace ConsoleApp
{
    internal class WebApiClient : IDisposable
    {
        private HttpClient _httpClient;

        public WebApiClient(string baseAddress)
        {

            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Brotli | System.Net.DecompressionMethods.Deflate,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls13 | System.Security.Authentication.SslProtocols.Tls12

            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("br"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));
        
            
        }



        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<T?> GetAsync<T>(string resource)
        {
            var result = await _httpClient.GetAsync(resource);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<T>();
        }
        public async Task<T> GetAsync<T>(string resource, int id)
        {
            var result = await _httpClient.GetAsync($"{resource}/{id}");
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<T>();
        }

        public async Task PutAsync<T>(string resource, int id, T item)
        {
            var result = await _httpClient.PutAsJsonAsync($"{resource}/{id}", item);
            result.EnsureSuccessStatusCode();
        }

        public async Task PostAsync<T>(string resource, T item)
        {
            var result = await _httpClient.PostAsJsonAsync(resource, item);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string resource, int id)
        {
            var result = await _httpClient.DeleteAsync($"{resource}/{id}");
            result.EnsureSuccessStatusCode();
        }

    }
}
