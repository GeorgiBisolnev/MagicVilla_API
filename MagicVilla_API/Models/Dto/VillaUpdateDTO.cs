using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    public class VillaUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int SquareFoot { get; set; }
        public int Occupancy { get; set; }


        public string? ImageURL { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageLocalPath { get; set; }
        public string Amenity { get; set; }
        //public DateTime CreatedDate { get; set; }
        //[Required]
        //public DateTime UpdatedDate { get; set; }

    }
}
