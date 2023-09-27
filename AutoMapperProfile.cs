using AutoMapper;
using PartsWebApi.Dtos;
using PartsWebApi.Entities;

namespace PartsWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            CreateMap<Part, PartDto>()
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination.DestinationTypeId))
                .ReverseMap();
        }
    }
}
