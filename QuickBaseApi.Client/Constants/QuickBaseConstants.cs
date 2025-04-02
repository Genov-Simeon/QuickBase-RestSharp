public static class QuickBaseConstants
{
    public const string DefaultBaseUrl = "https://api.quickbase.com/v1";
    public const int DefaultTimeout = 30000; // 30 seconds

    public static class Headers
    {
        public const string RealmHostname = "QB-Realm-Hostname";
        public const string Authorization = "Authorization";
        public const string UserToken = "QB-USER-TOKEN";
        public const string AppToken = "QB-APP-TOKEN";
    }
} 