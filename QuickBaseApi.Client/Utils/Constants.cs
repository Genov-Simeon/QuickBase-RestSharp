namespace QuickBaseApi.Client.Utils
{
    public static class Constants
    {
        public static readonly ErrorResponseModel InvalidUserToken = new ErrorResponseModel
        {
            Message = "Access denied",
            Description = "User token is invalid"
        };
    }
}
