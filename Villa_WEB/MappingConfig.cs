using AutoMapper;
using Villa_WEB.Models.Dto;

namespace Villa_WEB
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO,VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO,VillaNumberUpdateDTO>().ReverseMap(); 
            CreateMap<VillaNumberDTO, VillaNumberDeleteDTO>().ReverseMap();
        }
    }
}
