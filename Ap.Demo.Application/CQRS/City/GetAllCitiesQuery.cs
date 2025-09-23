using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ap.Demo.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Ap.Demo.Application.CQRS.City
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
    {
        public string SortOrder { get; set; } = "asc";
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
    {
        private readonly IUnitofWork _uow;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(IUnitofWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _uow.Cities.GetAll(request.SortOrder);
            return _mapper.Map<IEnumerable<CityDto>>(cities);
        }
    }
}
