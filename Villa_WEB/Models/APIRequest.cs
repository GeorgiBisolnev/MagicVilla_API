using static Villa_WEB.Data.Common.SD;

namespace Villa_WEB.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
