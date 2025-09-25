using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ap.Demo.Application.CQRS.City;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AP.Demo.Unittests.Cities
{
    [TestClass]
    public class DeleteCityQueryTests
    {
        private Mock<IUnitofWork> _uowMock = null!;
        private Mock<ICityRepository> _cityRepoMock = null!;
        private Mock<IEmailService> _emailServiceMock = null!;
        private DeleteCityQueryHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _uowMock = new Mock<IUnitofWork>();
            _cityRepoMock = new Mock<ICityRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            
            _uowMock.Setup(u => u.CityRepository).Returns(_cityRepoMock.Object);
            
            _handler = new DeleteCityQueryHandler(_uowMock.Object, _emailServiceMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidCity_DeletesCityAndSendsEmail()
        {
            // Arrange
            var cityId = 1;
            var cityToDelete = new City 
            { 
                Id = cityId, 
                Name = "Test City", 
                Population = 100000,
                CountryId = 1
            };
            
            var allCities = new List<City> 
            { 
                cityToDelete,
                new City { Id = 2, Name = "Another City", Population = 50000, CountryId = 1 }
            };

            _cityRepoMock.Setup(r => r.GetById(cityId))
                        .ReturnsAsync(cityToDelete);
            
            // Mock both the parameterless and parameterized versions
            _cityRepoMock.Setup(r => r.GetAll(It.IsAny<string>()))
                        .ReturnsAsync(allCities);
            _cityRepoMock.As<IGenericRepository<City>>().Setup(r => r.GetAll())
                        .ReturnsAsync(allCities);

            _uowMock.Setup(u => u.Commit())
                   .Returns(Task.CompletedTask);

            _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);

            var query = new DeleteCityQuery(cityId);

            // Act
            var cancellationToken = CancellationToken.None;
            await _handler.Handle(query, cancellationToken);

            // Assert
            _cityRepoMock.Verify(r => r.GetById(cityId), Times.Once);
            _cityRepoMock.Verify(r => r.GetAll(It.IsAny<string>()), Times.Once);
            _cityRepoMock.Verify(r => r.Delete(cityToDelete), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
            
            // Verify email was sent with correct parameters
            _emailServiceMock.Verify(e => e.SendEmailAsync(
                "03ayv21@gmail.com",
                "Stad Verwijderd - Systeem Notificatie",
                It.Is<string>(body => 
                    body.Contains("Test City") && 
                    body.Contains("100,000") && 
                    body.Contains("Een stad is verwijderd uit het systeem")
                )
            ), Times.Once);
        }

        [TestMethod]
        public async Task Handle_CityNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cityId = 999;
            
            _cityRepoMock.Setup(r => r.GetById(cityId))
                        .ReturnsAsync((City?)null);

            var query = new DeleteCityQuery(cityId);

            // Act & Assert
            var cancellationToken = CancellationToken.None;
            var exception = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                () => _handler.Handle(query, cancellationToken)
            );

            Assert.AreEqual("City 999 not found.", exception.Message);
            
            // Verify no deletion or email was attempted
            _cityRepoMock.Verify(r => r.Delete(It.IsAny<City>()), Times.Never);
            _uowMock.Verify(u => u.Commit(), Times.Never);
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_LastCityInDatabase_ThrowsInvalidOperationException()
        {
            // Arrange
            var cityId = 1;
            var lastCity = new City 
            { 
                Id = cityId, 
                Name = "Last City", 
                Population = 50000,
                CountryId = 1
            };
            
            var allCities = new List<City> { lastCity }; // Only one city

            _cityRepoMock.Setup(r => r.GetById(cityId))
                        .ReturnsAsync(lastCity);
            
            // Mock both the parameterless and parameterized versions
            _cityRepoMock.Setup(r => r.GetAll(It.IsAny<string>()))
                        .ReturnsAsync(allCities);
            _cityRepoMock.As<IGenericRepository<City>>().Setup(r => r.GetAll())
                        .ReturnsAsync(allCities);

            var query = new DeleteCityQuery(cityId);

            // Act & Assert
            var cancellationToken = CancellationToken.None;
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _handler.Handle(query, cancellationToken)
            );

            Assert.AreEqual("Cannot delete the last city in the database.", exception.Message);
            
            // Verify no deletion or email was attempted
            _cityRepoMock.Verify(r => r.Delete(It.IsAny<City>()), Times.Never);
            _uowMock.Verify(u => u.Commit(), Times.Never);
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_EmailServiceFails_StillDeletesCity()
        {
            // Arrange
            var cityId = 1;
            var cityToDelete = new City 
            { 
                Id = cityId, 
                Name = "Test City", 
                Population = 75000,
                CountryId = 1
            };
            
            var allCities = new List<City> 
            { 
                cityToDelete,
                new City { Id = 2, Name = "Another City", Population = 50000, CountryId = 1 }
            };

            _cityRepoMock.Setup(r => r.GetById(cityId))
                        .ReturnsAsync(cityToDelete);
            
            // Mock both the parameterless and parameterized versions
            _cityRepoMock.Setup(r => r.GetAll(It.IsAny<string>()))
                        .ReturnsAsync(allCities);
            _cityRepoMock.As<IGenericRepository<City>>().Setup(r => r.GetAll())
                        .ReturnsAsync(allCities);

            _uowMock.Setup(u => u.Commit())
                   .Returns(Task.CompletedTask);

            // Email service throws exception
            _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Email service unavailable"));

            var query = new DeleteCityQuery(cityId);

            // Act & Assert
            // This test should verify that the email exception is thrown
            var cancellationToken = CancellationToken.None;
            var exception = await Assert.ThrowsExceptionAsync<Exception>(
                () => _handler.Handle(query, cancellationToken)
            );

            Assert.AreEqual("Email service unavailable", exception.Message);
            
            // Verify city deletion occurred before the email failure
            _cityRepoMock.Verify(r => r.Delete(cityToDelete), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ValidCity_EmailContainsCorrectCityInformation()
        {
            // Arrange
            var cityId = 5;
            var cityToDelete = new City 
            { 
                Id = cityId, 
                Name = "Antwerpen", 
                Population = 523248,
                CountryId = 1
            };
            
            var allCities = new List<City> 
            { 
                cityToDelete,
                new City { Id = 6, Name = "Gent", Population = 262219, CountryId = 1 }
            };

            _cityRepoMock.Setup(r => r.GetById(cityId))
                        .ReturnsAsync(cityToDelete);
            
            // Mock both the parameterless and parameterized versions
            _cityRepoMock.Setup(r => r.GetAll(It.IsAny<string>()))
                        .ReturnsAsync(allCities);
            _cityRepoMock.As<IGenericRepository<City>>().Setup(r => r.GetAll())
                        .ReturnsAsync(allCities);

            _uowMock.Setup(u => u.Commit())
                   .Returns(Task.CompletedTask);

            string capturedEmailBody = string.Empty;
            _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Callback<string, string, string>((to, subject, body) => capturedEmailBody = body)
                           .Returns(Task.CompletedTask);

            var query = new DeleteCityQuery(cityId);

            // Act
            var cancellationToken = CancellationToken.None;
            await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.IsTrue(capturedEmailBody.Contains("Antwerpen"), "Email body should contain city name");
            Assert.IsTrue(capturedEmailBody.Contains("523,248"), "Email body should contain formatted population");
            Assert.IsTrue(capturedEmailBody.Contains("Een stad is verwijderd uit het systeem"), "Email body should contain deletion message");
            Assert.IsTrue(capturedEmailBody.Contains("Deze actie kan niet ongedaan worden gemaakt"), "Email body should contain warning message");
            Assert.IsTrue(capturedEmailBody.Contains(DateTime.UtcNow.ToString("yyyy-MM-dd")), "Email body should contain current date");
        }
    }
}
