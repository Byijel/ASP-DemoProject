using Ap.Demo.Application.Exceptions;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Domain;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ap.Demo.Application.CQRS.City
{
    public class UpdateCitiesCommand : IRequest<UpdateCityDTO>
    {
        public UpdateCityDTO City { get; set; }
    }

    public class UpdateCitiesCommandValidator : AbstractValidator<UpdateCitiesCommand>
    {
        private readonly IUnitofWork _uow;

        public UpdateCitiesCommandValidator(IUnitofWork uow)
        {
            _uow = uow;
            RuleFor(s => s.City.Population)
                .GreaterThan(0)
                .WithMessage("Population must be positive")
                .LessThan(1000000000)
                .WithMessage("Population cannot exceed 1 billion");
        }
    }

    public class UpdateCitiesCommandHandler : IRequestHandler<UpdateCitiesCommand, UpdateCityDTO>
    {
        private readonly IUnitofWork uow;
        private readonly IMapper mapper;

        public UpdateCitiesCommandHandler(IUnitofWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<UpdateCityDTO> Handle(UpdateCitiesCommand request, CancellationToken cancellationToken)
        {
            var existingCity = await uow.CityRepository.GetById(request.City.Id);
            if (existingCity == null) throw new NotFoundException("City not found");

            mapper.Map(request.City, existingCity);
            uow.CityRepository.Update(existingCity);
            await uow.Commit();

            return mapper.Map<UpdateCityDTO>(existingCity);
        }
    }
}