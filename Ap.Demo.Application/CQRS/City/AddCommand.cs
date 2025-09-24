using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ap.Demo.Application.Interfaces;



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
                .NotEmpty()
                .WithMessage("City name cannot be empty")
                .MustAsync(async (name, cancellation) => await UniqueCityName(name))
                .WithMessage("City name already exists in database");

            RuleFor(c => c.City.Population)
                .LessThanOrEqualTo(10_000_000_000)
                .WithMessage("Population must be less then 10 billion");

            RuleFor(c => c.City.CountryId)
                .NotEqual(0)
                .WithMessage("You must select a country from the dropdown menu");
        }

        private async Task<bool> UniqueCityName(string cityName)
        {
            var existingCity = await uow.CityRepository.GetByName(cityName);
            return existingCity == null;
        }


    }
}
