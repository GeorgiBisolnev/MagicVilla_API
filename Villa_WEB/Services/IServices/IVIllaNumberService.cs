using Villa_WEB.Models.Dto;

namespace Villa_WEB.Services.IServices
{
    public interface IVIllaNumberService
    {
        Task<T> GetAllAsyc<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsyn<T>(VillaNumberCreateDTO dto, string token);
        Task<T> UpdateAsyn<T>(VillaNumberUpdateDTO dto, string token);
        Task<T> DeleteAsyn<T>(int id, string token);
    }
}
