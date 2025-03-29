using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client
{
    public class QuickBaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly QuickBaseConfig _config;
        private string _authTicket;

        public QuickBaseClient(QuickBaseConfig config   )
        {
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("QB-Realm-Hostname", config.Realm);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"QB-USER-TOKEN {config.UserToken}");
            _httpClient.DefaultRequestHeaders.Add("QB-APP-TOKEN", config.AppToken);
        }

        public async Task<string> AuthenticateAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.BaseUrl}/auth");
            response.EnsureSuccessStatusCode();
            _authTicket = await response.Content.ReadAsStringAsync();
            return _authTicket;
        }

        public async Task<string> AddRecordAsync(string tableId, object record)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(record),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(
                $"{_config.BaseUrl}/records/{tableId}",
                content);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> EditRecordAsync(string tableId, string recordId, object record)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(record),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync(
                $"{_config.BaseUrl}/records/{tableId}/{recordId}",
                content);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task DeleteRecordAsync(string tableId, string recordId)
        {
            var response = await _httpClient.DeleteAsync(
                $"{_config.BaseUrl}/records/{tableId}/{recordId}");

            response.EnsureSuccessStatusCode();
        }
    }
} 