using Ap.Demo.Domain;
using Ap.Demo.Application.CQRS; 
using Ap.Demo.Application.CQRS.City;
using AutoMapper;
using Ap.Demo.Application.CQRS.Country;

namespace Ap.Demo.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, CityDTO>();

            CreateMap<CityDTO, City>()
                .ForMember(dest => dest.Country, opt => opt.Ignore());
                
            CreateMap<Country, CountryDTO>();

            CreateMap<UpdateCityDTO, City>()
                .ForMember(s => s.Id, p => p.Ignore())
                .ForMember(s => s.Name, p => p.Ignore())
                .ForMember(s => s.CountryId, p => p.Ignore())
                .ForMember(s => s.Country, p => p.Ignore());

            CreateMap<City, UpdateCityDTO>();
        }

    }
}