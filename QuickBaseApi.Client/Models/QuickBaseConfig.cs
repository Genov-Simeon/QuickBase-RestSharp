using System;

namespace QuickBaseApi.Client.Models
{
    public class QuickBaseConfig
    {
        public string Realm { get; set; }
        public string UserToken { get; set; }
        public string AppToken { get; set; }
        public string BaseUrl { get; set; } = "https://api.quickbase.com/v1";
    }
} 