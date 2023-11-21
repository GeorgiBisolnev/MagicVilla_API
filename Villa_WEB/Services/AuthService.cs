using System.Reflection;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Services
{
    public class AuthService : BaseService , IAuthService
    {
        private readonly string _urlAPI;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            this._urlAPI = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            this._httpClientFactory = httpClientFactory;
        }
        public Task<T> LoginAsync<T>(LoginRequestDTO model)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = Data.Common.SD.ApiType.POST,
                Data = model,
                Url = this._urlAPI + "/api/UsersAuth/login",
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO model)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = Data.Common.SD.ApiType.POST,
                Data = model,
                Url = this._urlAPI + "/api/UsersAuth/register"
            });
        }
    }
}
