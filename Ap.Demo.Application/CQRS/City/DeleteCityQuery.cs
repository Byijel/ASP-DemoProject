using MediatR;
using Ap.Demo.Application.Interfaces;

namespace Ap.Demo.Application.CQRS.City
{
    public record DeleteCityCommand(int Id) : IRequest;

    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
    {
        private readonly IUnitofWork _uow;

        public DeleteCityCommandHandler(IUnitofWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _uow.Cities.GetById(request.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"City {request.Id} not found.");
            }

            _uow.Cities.Delete(entity);
            await _uow.Commit();
        }
    }
}