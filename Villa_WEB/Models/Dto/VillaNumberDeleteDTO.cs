using System.ComponentModel.DataAnnotations;

namespace Villa_WEB.Models.Dto
{
    public class VillaNumberDeleteDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string Details { get; set; }

    }
}
