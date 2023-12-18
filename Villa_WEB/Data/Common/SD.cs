namespace Villa_WEB.Data.Common
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static string SessionToken = "JWTToken";
        public static string  Admin = "admin";
        public static string  Customer = "customer";
        public enum ContentType
        {
            Json,
            MultipartFormData
        }
    }
}
