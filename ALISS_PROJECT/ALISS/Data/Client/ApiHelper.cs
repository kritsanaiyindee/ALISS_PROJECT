using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.Client
{
    public class ApiHelper
    {
        private static readonly ILogService log = new LogService(typeof(ApiHelper));

        public HttpClient _httpClient;// { get; private set; }

        public ApiHelper(string apiUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            //_httpClient = httpClient;
        }

        public async Task<List<T>> GetDataListAsync<T>(string apiName)
        {
            log.MethodStart();
            var response = await _httpClient.GetAsync(apiName);
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();

            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<List<T>>(responseContent);
        }

        public async Task<List<T>> GetDataListByParamsAsync<T>(string apiName, string param)
        {
            log.MethodStart();
            var response = await _httpClient.GetAsync($"{apiName}/{param}");
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<List<T>>(responseContent);
        }

        public async Task<T> GetDataByIdAsync<T>(string apiName, string obj_id)
        {
            log.MethodStart();
            var response = await _httpClient.GetAsync($"{apiName}/{obj_id}");
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<T>(responseContent);
        }
        public async Task<T> PostListofDataAsync<T>(string apiName, List<T> model)
        {
            log.MethodStart();
            var data = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiName}", content);
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<T>(responseContent);
        }
        public async Task<T> PostDataAsync<T>(string apiName, T model)
        {
            log.MethodStart();
            var data = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiName}", content);
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<T>(responseContent);
        }

        public async Task<T> GetDataByModelAsync<T, TT>(string apiName, TT model)
        {
            log.MethodStart();
            var data = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiName}", content);
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<T>(responseContent);
        }

        public async Task<List<T>> GetDataListByModelAsync<T, TT>(string apiName, TT model)
        {
            log.MethodStart();
            var data = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiName}", content);
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            log.MethodFinish();
            return await JsonSerializer.DeserializeAsync<List<T>>(responseContent);
        }

    }
}
