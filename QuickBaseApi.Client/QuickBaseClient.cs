using Newtonsoft.Json;
using QuickBaseApi.Client.Models;
using RestSharp;

namespace QuickBaseApi.Client
{
    public class QuickBaseClient
    {
        private readonly RestClient? _restClient;

        public QuickBaseClient(QuickBaseConfig config)
        {
            var options = new RestClientOptions(config.BaseUrl);
            _restClient = new RestClient(options);

            _restClient.AddDefaultHeader("QB-Realm-Hostname", config.Realm);
            _restClient.AddDefaultHeader("Authorization", $"QB-USER-TOKEN {config.UserToken}");
        }

        public async Task<List<QuickBaseTable>> GetTablesAsync(string appId)
        {
            var request = new RestRequest("/tables", Method.Get)
                .AddQueryParameter("appId", appId);

            var response = await _restClient.ExecuteGetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to get tables: {response.ErrorMessage}");
            }

            var tables = JsonConvert.DeserializeObject<List<QuickBaseTable>>(response.Content);

            return tables ?? new List<QuickBaseTable>();
        }

        public async Task<List<QuickBaseFieldModel>> GetFieldsByTableName(string tableId)
        {
            var request = new RestRequest("/fields", Method.Get)
                .AddQueryParameter("tableId", tableId);

            var response = await _restClient.ExecuteGetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to fetch fields for table {tableId}: {response.ErrorMessage}");
            }

            var fields = JsonConvert.DeserializeObject<List<QuickBaseFieldModel>>(response.Content);

            return fields ?? new List<QuickBaseFieldModel>();
        }

        public async Task<RestResponse> PostRecordAsync(object requestBody, string? userToken = null)
        {
            var request = new RestRequest("/records", Method.Post);
            request.AddJsonBody(requestBody);

            if (!string.IsNullOrWhiteSpace(userToken))
            {
                request.AddOrUpdateHeader("Authorization", $"QB-USER-TOKEN {userToken}");
            }

            var response = await _restClient.ExecuteAsync(request);

            return response;
        }

        public async Task<RestResponse> DeleteRecordAsync(object requestBody)
        {
            var request = new RestRequest("/records", Method.Delete);
            request.AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request);
            
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to edit record: {response.Content}");
            }

            return response;
        }
    }
} 