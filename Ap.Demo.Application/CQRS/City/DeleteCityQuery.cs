using MediatR;
using Ap.Demo.Application.Interfaces;

namespace Ap.Demo.Application.CQRS.City
{
    public record DeleteCityQuery(int Id) : IRequest;

    public class DeleteCityQueryHandler : IRequestHandler<DeleteCityQuery>
    {
        private readonly IUnitofWork _uow;

        public DeleteCityQueryHandler(IUnitofWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteCityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _uow.CityRepository.GetById(request.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"City {request.Id} not found.");
            }

            _uow.CityRepository.Delete(entity);
            await _uow.Commit();
        }
    }
}