﻿using System.ComponentModel.DataAnnotations;

namespace Villa_WEB.Models.Dto
{
    public class UserDTO
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
