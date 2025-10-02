using Ap.Demo.Application.CQRS.City;
using Ap.Demo.Application.Interfaces;
using Moq;

namespace Ap.Demo.UnitTests
{
    [TestClass]
    public class UpdateCitiesCommandTests
    {
        private Mock<IUnitofWork> _mockUow;
        private UpdateCitiesCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _mockUow = new Mock<IUnitofWork>();
            _validator = new UpdateCitiesCommandValidator(_mockUow.Object);
        }



        [TestMethod]
        public void Validate_NegativePopulation_ReturnsFalse()
        {
            var command = new UpdateCitiesCommand { City = new UpdateCityDTO { Id = 1, Population = -1 } };
            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Population must be positive"));
        }

        [TestMethod]
        public void Validate_PopulationExceedsLimit_ReturnsFalse()
        {
            var command = new UpdateCitiesCommand { City = new UpdateCityDTO { Id = 1, Population = 1000000000 } };
            var result = _validator.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Population cannot exceed 1 billion"));
        }

        [TestMethod]
        public void Validate_ValidInput_ReturnsTrue()
        {
            var command = new UpdateCitiesCommand { City = new UpdateCityDTO { Id = 1, Population = 500000 } };
            var result = _validator.Validate(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}