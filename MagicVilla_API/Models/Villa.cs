using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }  
    }
}
