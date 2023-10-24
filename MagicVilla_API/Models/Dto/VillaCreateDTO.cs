using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    public class VillaCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int SquareFoot { get; set; }
        public int Occupancy { get; set; }
        public string ImageURL { get; set; }
        public string Amenity { get; set; }

    }
}
