using System.ComponentModel.DataAnnotations;

namespace Villa_WEB.Models.Dto
{
    public class LoginRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
