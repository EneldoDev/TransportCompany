using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Commands.EditTruck;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Commands.EditTruck
{
    public class EditTruckCommandHandlerTests
    {
        private readonly Mock<ITrucksRepository> _trucksRepositoryMock;

        public EditTruckCommandHandlerTests()
        {
            _trucksRepositoryMock = new ();
        }

        [Fact]
        public async Task Handle_Should_ReturnConflict_WhenCodeIsNotUnique()
        {
            //Arrange
            var command = new EditTruckCommand(Guid.NewGuid(), "Code", "Name", "Description", TruckStatus.OutOfService);
            _trucksRepositoryMock.Setup(x => x.HasUniqueCode(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var commandHandler = new EditTruckCommandHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.Conflict);
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_WhenTruckDoesNotExist()
        {
            //Arrange
            var command = new EditTruckCommand(Guid.NewGuid(), "Code", "Name", "Description", TruckStatus.OutOfService);
            _trucksRepositoryMock.Setup(x => x.HasUniqueCode(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var commandHandler = new EditTruckCommandHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenDataIsValid()
        {
            //Arrange
            var truck = new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning };
            var command = new EditTruckCommand(truck.Id, "Code", "Name", "Description", TruckStatus.OutOfService);
            _trucksRepositoryMock.Setup(x => x.HasUniqueCode(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _trucksRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id, default)).ReturnsAsync(truck);
            _trucksRepositoryMock.Setup(x => x.UpdateAsync(truck, default));

            var commandHandler = new EditTruckCommandHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().Be(Result.Success);
        }
    }
}
