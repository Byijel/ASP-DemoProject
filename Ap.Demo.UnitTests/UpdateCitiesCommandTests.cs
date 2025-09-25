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

        /*        [TestMethod]
                public void Validate_NullName_ReturnsFalse()
                {
                    var command = new UpdateCitiesCommand { City = new CityDto { Name = null, Population = 1000 } };
                    var result = _validator.Validate(command);

                    Assert.IsFalse(result.IsValid);
                    Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Name cannot be NULL"));
                }

                [TestMethod]
                public void Validate_NameTooLong_ReturnsFalse()
                {
                    var command = new UpdateCitiesCommand { City = new CityDto { Name = new string('A', 16), Population = 1000 } };
                    var result = _validator.Validate(command);

                    Assert.IsFalse(result.IsValid);
                    Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Name can be no more than 15 chars"));
                }*/

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