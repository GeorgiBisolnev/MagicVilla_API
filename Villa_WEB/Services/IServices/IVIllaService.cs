using Villa_WEB.Models.Dto;

namespace Villa_WEB.Services.IServices
{
    public interface IVIllaService
    {
        Task<T> GetAllAsyc<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsyn<T>(VillaCreateDTO dto, string token);
        Task<T> UpdateAsyn<T>(VillaUpdateDTO dto, string token);
        Task<T> DeleteAsyn<T>(int id, string token);
    }
}
