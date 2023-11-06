using Villa_WEB.Models.Dto;

namespace Villa_WEB.Services.IServices
{
    public interface IVIllaNumberService
    {
        Task<T> GetAllAsyc<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsyn<T>(VillaNumberCreateDTO dto);
        Task<T> UpdateAsyn<T>(VillaNumberUpdateDTO dto);
        Task<T> DeleteAsyn<T>(int id);
    }
}
