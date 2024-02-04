using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Commands.UpdateTruckStatus;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Commands.UpdateTruckStatus
{
    public class UpdateTruckStatusCommandHandlerTests
    {
        private readonly Mock<ITrucksRepository> _truckRepositoryMock;

        public UpdateTruckStatusCommandHandlerTests()
        {
            _truckRepositoryMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_WhenTruckNotFound()
        {
            //Arrange
            var command = new UpdateTruckStatusCommand(Guid.NewGuid(), TruckStatus.AtJob);
            var commandHandler = new UpdateTruckStatusCommandHandler(_truckRepositoryMock.Object);

            //Act
            var result = await commandHandler.Handle(command, default);

            //Asset
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.NotFound);
        }


        [Fact]
        public async Task Handle_Should_ReturnValidationError_WhenInvalidStatusFlow()
        {
            //Arrange
            var truck = new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning };
            var command = new UpdateTruckStatusCommand(truck.Id, TruckStatus.AtJob);
            var commandHandler = new UpdateTruckStatusCommandHandler(_truckRepositoryMock.Object);
            _truckRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(truck);
            //Act
            var result = await commandHandler.Handle(command, default);

            //Asset
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().HaveFlag(ErrorType.Validation);
            result.FirstError.Code.Should().BeEquivalentTo("Unable to make requested truck status transition");
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenValidStatusFlow()
        {
            //Arrange
            var truck = new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning };
            var command = new UpdateTruckStatusCommand(truck.Id, TruckStatus.Loading);
            var commandHandler = new UpdateTruckStatusCommandHandler(_truckRepositoryMock.Object);
            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id, default)).ReturnsAsync(truck);
            _truckRepositoryMock.Setup(x => x.UpdateAsync(truck, default));
            //Act
            var result = await commandHandler.Handle(command, default);

            //Asset
            result.IsError.Should().BeFalse();
            result.Value.Should().Be(Result.Success);
        }
    }
}
