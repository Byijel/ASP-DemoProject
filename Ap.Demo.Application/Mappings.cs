using Ap.Demo.Domain;
using Ap.Demo.Application.CQRS; 
using Ap.Demo.Application.CQRS.City;
using AutoMapper;

namespace Ap.Demo.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name));
        }
    }
}