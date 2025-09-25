using Ap.Demo.Application.Exceptions;
using Ap.Demo.Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Application.CQRS.City
{
    public class GetCityByIdQuery : IRequest<CityDto>
    {
        public int Id { get; set; }
    }

    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto>
    {
        private readonly IUnitofWork _uow;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(IUnitofWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var city = await _uow.Cities.GetById(request.Id);
            if (city == null) throw new NotFoundException("City not found");
            return _mapper.Map<CityDto>(city);
        }
    }
}
