using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO() { Id=1,Name="Pool View",SquareFoot=100, Occupancy=20},
                new VillaDTO() { Id=2,Name="Beach View",SquareFoot=125, Occupancy=25},
                new VillaDTO() { Id=3,Name="Mountain View",SquareFoot=150, Occupancy=30},
                new VillaDTO() { Id=4,Name="Vihren",SquareFoot=175, Occupancy=32},
            };

    }
}
