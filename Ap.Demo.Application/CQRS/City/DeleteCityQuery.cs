using MediatR;
using Ap.Demo.Application.Interfaces;
using System.Linq;
using Ap.Demo.Application.Configuration;
using Microsoft.Extensions.Options;

namespace Ap.Demo.Application.CQRS.City
{
    public record DeleteCityQuery(int Id) : IRequest;

    public class DeleteCityQueryHandler : IRequestHandler<DeleteCityQuery>
    {
        private readonly IUnitofWork _uow;
        private readonly IEmailService _emailService;
        private readonly NotificationSettings _notificationSettings;

        public DeleteCityQueryHandler(IUnitofWork uow, IEmailService emailService, IOptions<NotificationSettings> notificationSettings)
        {
            _uow = uow;
            _emailService = emailService;
            _notificationSettings = notificationSettings.Value;
        }

        public async Task Handle(DeleteCityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _uow.CityRepository.GetById(request.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"City {request.Id} not found.");
            }

            var allCities = await _uow.CityRepository.GetAll();
            if (allCities.Count() <= 1)
            {

                throw new InvalidOperationException("Cannot delete the last city in the database.");
            }

            _uow.CityRepository.Delete(entity);
            await _uow.Commit();

            // send email notification
            const string adminEmail = "03ayv21@gmail.com"; //admin test email
            const string subject = "City Deleted - System Notification";
            string body = $"A city has been deleted from the system.\n\n" +
                         $"City name: {entity.Name}\n" +
                         $"Population: {entity.Population:N0}\n" +
                         $"Deleted on: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n\n" +
                         $"This action cannot be undone.";
            await _emailService.SendEmailAsync(adminEmail, subject, body);
        }
    }
}