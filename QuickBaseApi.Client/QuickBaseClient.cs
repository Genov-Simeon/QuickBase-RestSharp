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

            _restClient.AddDefaultHeader(QuickBaseConstants.Headers.RealmHostname, config.Realm);
            _restClient.AddDefaultHeader(QuickBaseConstants.Headers.Authorization, $"{QuickBaseConstants.Headers.UserToken} {config.UserToken}");
        }

        public async Task<List<TableModel>> GetTablesAsync(string appId)
        {
            var request = new RestRequest("/tables", Method.Get)
                .AddQueryParameter("appId", appId);

            var response = await _restClient.ExecuteGetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to get tables: {response.ErrorMessage}");
            }

            var tables = JsonConvert.DeserializeObject<List<TableModel>>(response.Content);
            return tables ?? new List<TableModel>();
        }

        public async Task<List<FieldModel>> GetFieldsByTableName(string tableId)
        {
            var request = new RestRequest("/fields", Method.Get)
                .AddQueryParameter("tableId", tableId);

            var response = await _restClient.ExecuteGetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to fetch fields for table {tableId}: {response.ErrorMessage}");
            }

            var fields = JsonConvert.DeserializeObject<List<FieldModel>>(response.Content);
            return fields ?? new List<FieldModel>();
        }

        public async Task<RestResponse> PostRecordAsync(object requestBody, string? userToken = null)
        {
            var request = new RestRequest("/records", Method.Post)
                .AddJsonBody(requestBody);
            
            var response = await _restClient.ExecuteAsync(request);

            if (!string.IsNullOrWhiteSpace(userToken))
            {
                request.AddOrUpdateHeader(QuickBaseConstants.Headers.Authorization, $"{QuickBaseConstants.Headers.UserToken} {userToken}");
            }

            return response;
        }

        public async Task<RestResponse> DeleteRecordAsync(object requestBody)
        {
            var request = new RestRequest("/records", Method.Delete)
                .AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to delete record: {response.Content}");
            }

            return response;
        }
    }
}
