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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore()); 

            CreateMap<Country, CountryDTO>();


        }
    }
}