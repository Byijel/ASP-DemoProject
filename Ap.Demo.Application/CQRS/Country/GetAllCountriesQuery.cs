using Ap.Demo.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ap.Demo.Application.CQRS.Country
{
    public class GetAllCountriesQuery : IRequest<IEnumerable<CountryDTO>>
    {
    }

    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDTO>>
    {
        private readonly IUnitofWork uow;
        private readonly IMapper mapper;

        public GetAllCountriesQueryHandler(IUnitofWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CountryDTO>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<IEnumerable<CountryDTO>>(await uow.CountryRepository.GetAll());
        }
    }
}
