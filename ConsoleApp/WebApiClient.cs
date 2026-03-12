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

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

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
