using Villa_WEB.Models.Dto;

namespace Villa_WEB.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
    }
}
