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
            //read only name
            CreateMap<CityDto, City>()
                .ForMember(s => s.Name, s => s.Ignore())
                .ForMember(dest => dest.CountryId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Country, opt => opt.Ignore());
        }

    }
}