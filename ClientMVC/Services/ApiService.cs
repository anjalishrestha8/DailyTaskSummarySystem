using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClientMVC.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly IHttpContextAccessor httpContextAccessor;
        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = httpClientFactory.CreateClient("ClientWebApi");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse?> GetAsync<TResponse>(string uri)
        {
            try
            {
                var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    //return await response.Content.ReadFromJsonAsync<TResponse>();
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
                }
                return default;
            }
            catch (HttpRequestException)
            {
                return default;
            }

        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    //return await response.Content.ReadFromJsonAsync<TResponse>();
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);

                }
                return default;
            }
            catch (HttpRequestException)
            {
                return default;
            }

        }
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            try
            {
                var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    //return await response.Content.ReadFromJsonAsync<TResponse>();
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
                }
                return default;
            }
            catch (HttpRequestException)
            {
                return default;
            }

        }
        public async Task<TResponse?> DeleteAsync<TResponse>(string uri)
        {
            try
            {
                var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await httpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    //return await response.Content.ReadFromJsonAsync<TResponse>();
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
                }
                return default;
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
    }
}
