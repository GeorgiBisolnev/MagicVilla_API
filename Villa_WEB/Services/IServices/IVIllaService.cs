using Villa_WEB.Models.Dto;

namespace Villa_WEB.Services.IServices
{
    public interface IVIllaService
    {
        Task<T> GetAllAsyc<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsyn<T>(VillaCreateDTO dto);
        Task<T> UpdateAsyn<T>(VillaUpdateDTO dto);
        Task<T> DeleteAsyn<T>(int id);
    }
}
