using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Commands.CreateTruck;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Commands.CreateTruck
{
    public class CreateTruckCommandHandlerTests
    {
        private readonly Mock<ITrucksRepository> _trucksRepositoryMock;

        public CreateTruckCommandHandlerTests()
        {
            _trucksRepositoryMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnConflictCode_WhenCodeNotUniqe()
        {
            //Arrange
            var command = new CreateTruckCommand("Code", "Name", "Description");
            _trucksRepositoryMock.Setup(x => x.HasUniqueCode(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var commandHandler = new CreateTruckCommandHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.Conflict);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessCode_WhenCodeIsUniqe()
        {
            //Arrange
            var command = new CreateTruckCommand("Code", "Name", "Description");
            _trucksRepositoryMock.Setup(x => x.HasUniqueCode(It.IsAny<string>(), default)).ReturnsAsync(true);
            _trucksRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Truck>(), It.IsAny<CancellationToken>()));
            var commandHandler = new CreateTruckCommandHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeEmpty();
        }
    }
}
