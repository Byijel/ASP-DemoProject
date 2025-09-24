using System.Threading.Tasks;
using Ap.Demo.Application.CQRS.City;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Domain;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ap.Demo.Tests.City
{
    [TestClass]
    public class AddCommandTests
    {
        private Mock<IUnitofWork> uowMock = null!;
        private Mock<ICityRepository> cityRepoMock = null!;

        [TestInitialize]
        public void Setup()
        {
            uowMock = new Mock<IUnitofWork>();
            cityRepoMock = new Mock<ICityRepository>();
            uowMock.Setup(u => u.CityRepository).Returns(cityRepoMock.Object);
        }

        [TestMethod]
        public async Task Name_Is_Required()
        {
            // Arrange
            var dto = new CityDTO { Name = "", Population = 100, CountryId = 1 };
            var unit = new AddCommand { City = dto };

            // Act
            var result = await new AddCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.City.Name);
        }

        [TestMethod]
        public async Task Population_Must_Be_At_Most_10_Billion()
        {
            // Arrange
            var dto = new CityDTO { Name = "Test", Population = 10_000_000_001L, CountryId = 1 };
            var unit = new AddCommand { City = dto };

            // Act
            var result = await new AddCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.City.Population);
        }

        [TestMethod]
        public async Task Country_Must_Be_Selected()
        {
            // Arrange
            var dto = new CityDTO { Name = "Test", Population = 100, CountryId = 0 };
            var unit = new AddCommand { City = dto };

            // Act
            var result = await new AddCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.City.CountryId);
        }

        [TestMethod]
        public async Task Duplicate_Name_In_Same_Country_Fails()
        {
            // Arrange
            var dto = new CityDTO { Name = "Gent", Population = 100, CountryId = 1 };
            var unit = new AddCommand { City = dto };

            // simulate existing city with same name + country
            cityRepoMock
                .Setup(r => r.GetByNameAndCountryId("Gent", 1))
                .ReturnsAsync(new Ap.Demo.Domain.City { Id = 42, Name = "Gent", CountryId = 1 });

            // Act
            var result = await new AddCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.City)
                  .WithErrorMessage("City name already exists in selected country");
        }

        [TestMethod]
        public async Task Valid_Input_Passes()
        {
            // Arrange
            var dto = new CityDTO { Name = "Nieuwe Stad", Population = 1234, CountryId = 2 };
            var unit = new AddCommand { City = dto };

            // simulate unique
            cityRepoMock
                .Setup(r => r.GetByNameAndCountryId("Nieuwe Stad", 2))
                .ReturnsAsync((Ap.Demo.Domain.City?)null);

            // Act
            var result = await new AddCommandValidator(uowMock.Object)
                .TestValidateAsync(unit);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}