using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ap.Demo.Application.Interfaces;
using AutoMapper;
using Ap.Demo.Domain;



namespace Ap.Demo.Application.CQRS.City
{
    public class AddCommand : IRequest<CityDTO>
    {
        public CityDTO City { get; set; }
    }

    public class AddCommandValidator : AbstractValidator<AddCommand>
    {
        private IUnitofWork uow;

        public AddCommandValidator(IUnitofWork uow)
        {
            this.uow = uow;

            RuleFor(c => c.City.Name)
                .NotEmpty().WithMessage("City name cannot be empty");

            RuleFor(c => c.City.Population)
                .LessThanOrEqualTo(10_000_000_000L)
                .WithMessage("Population must be less then 10 billion");

            RuleFor(c => c.City.CountryId)
                .NotEqual(0)
                .WithMessage("You must select a country from the dropdown menu");

            RuleFor(c => c.City)
                .MustAsync(async (city, cancellation) =>
                    await uow.CityRepository.GetByNameAndCountryId(city.Name, city.CountryId) == null)
                .WithMessage("City name already exists in selected country");
        }
    }

    public class AddCommandHandler : IRequestHandler<AddCommand, CityDTO>
    {
        private readonly IUnitofWork uow;
        private readonly IMapper mapper;

        public AddCommandHandler(IUnitofWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        
        public async Task<CityDTO> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var cityEntity = mapper.Map<Ap.Demo.Domain.City>(request.City);

            var savedCity = await uow.CityRepository.Add(cityEntity);

            await uow.Commit();

            return mapper.Map<CityDTO>(savedCity);
        }
    }

}
