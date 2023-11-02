using Villa_WEB.Models;

namespace Villa_WEB.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set;  }

        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
