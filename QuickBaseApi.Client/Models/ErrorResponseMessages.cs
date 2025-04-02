namespace QuickBaseApi.Client.Utils
{
    public static class ErrorResponseMessages
    {
        public static readonly ErrorResponseModel InvalidUserToken = new ErrorResponseModel
        {
            Message = "Access denied",
            Description = "User token is invalid"
        };

        public static readonly ErrorResponseModel EmptyData = new ErrorResponseModel
        {
            Message = "Bad Request",
            Description = "'data' should NOT have fewer than 1 items"
        };

        public static readonly ErrorResponseModel InvalidData = new ErrorResponseModel
        {
            Message = "Bad Request",
            Description = "'data' should be array"
        };

        public static readonly ErrorResponseModel InvalidTableId = new ErrorResponseModel
        {
            Message = "Bad Request",
            Description = "'to' should be string"
        };

        public static readonly ErrorResponseModel MissingAuthorizationHeader  = new ErrorResponseModel
        {
            Message = "Bad Request",
            Description = "Required header 'authorization' not found"
        };
    }
}
