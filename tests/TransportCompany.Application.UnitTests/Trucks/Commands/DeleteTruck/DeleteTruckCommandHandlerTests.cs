using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Commands.DeleteTruck;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Commands.DeleteTruck
{
    public class DeleteTruckCommandHandlerTests
    {
        private readonly Mock<ITrucksRepository> _truckRepositoryMock;

        public DeleteTruckCommandHandlerTests()
        {
            _truckRepositoryMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccess_WhenTruckIsntDeleted()
        {
            //Arrange
            var truck = new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning };
            var command = new DeleteTruckCommand(truck.Id);
            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id, default)).ReturnsAsync(truck);
            _truckRepositoryMock.Setup(x => x.UpdateAsync(truck, default));
            var commandHandler = new DeleteTruckCommandHandler(_truckRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFound_WhenTruckNotFound()
        {
            //Arrange
            var command = new DeleteTruckCommand(Guid.NewGuid());
            var commandHandler = new DeleteTruckCommandHandler(_truckRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.NotFound);
        }
    }
}
