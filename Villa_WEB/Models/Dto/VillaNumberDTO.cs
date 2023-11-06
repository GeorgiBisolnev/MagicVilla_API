using System.ComponentModel.DataAnnotations;

namespace Villa_WEB.Models.Dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string Details { get; set; }
        public VillaDTO Villa { get; set; }
    }
}
