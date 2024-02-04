using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Queries.GetTruck;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Queries.GetTruck
{
    public class GetTruckQueryHandlerTests
    {
        private readonly Mock<ITrucksRepository> _trucksRepositoryMock;

        public GetTruckQueryHandlerTests()
        {
            _trucksRepositoryMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnNotFound_WhenTruckNotFound()
        {
            //Arrange
            var query = new GetTruckQuery(Guid.NewGuid());
            var queryHandler = new GetTruckQueryHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await queryHandler.Handle(query, default);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.HasFlag(ErrorType.NotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnTruck_WhenTruckIsFound()
        {
            //Arrange
            var truck = new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.AtJob };
            var query = new GetTruckQuery(truck.Id);
            var queryHandler = new GetTruckQueryHandler(_trucksRepositoryMock.Object);
            _trucksRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id, default)).ReturnsAsync(truck);
            //Act
            var result = await queryHandler.Handle(query, default);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<Truck>();
        }
    }
}
