using Microsoft.Extensions.Configuration;

namespace QuickBaseApi.Client
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot _config;

        public static IConfigurationRoot Config => _config;

        public static string BaseUrl => Get("BaseUrl");

        static Configuration()
        {
            var configFile = Directory.GetFiles(Directory.GetCurrentDirectory(), "appsettings.*.json").First();

            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .Build();
        }

        private static string Get(string key)
        {
            return _config[key];
        }
    }
}
