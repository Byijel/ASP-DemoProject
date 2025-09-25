using Ap.Demo.Application.Exceptions;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Domain;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ap.Demo.Application.CQRS.City
{
    public class UpdateCitiesCommand : IRequest<UpdateCityDto>
    {
        public UpdateCityDto City { get; set; }
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

    public class UpdateCitiesCommandHandler : IRequestHandler<UpdateCitiesCommand, UpdateCityDto>
    {
        private readonly IUnitofWork uow;
        private readonly IMapper mapper;

        public UpdateCitiesCommandHandler(IUnitofWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<UpdateCityDto> Handle(UpdateCitiesCommand request, CancellationToken cancellationToken)
        {
            var existingCity = await uow.Cities.GetById(request.City.Id);
            if (existingCity == null) throw new NotFoundException("City not found");

            mapper.Map(request.City, existingCity);
            uow.Cities.Update(existingCity);
            await uow.Commit();

            return mapper.Map<UpdateCityDto>(existingCity);
        }
    }
}