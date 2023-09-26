using AutoMapper;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi
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
