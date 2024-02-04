using TransportCompany.Application.Interfaces;
using TransportCompany.Application.Trucks.Queries.GetTrucksList;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.UnitTests.Trucks.Queries.GetTrucksList
{
    public class GetTrucksListQueryHandlerTests
    {
        private readonly Mock<ITrucksRepository> _trucksRepositoryMock;

        public GetTrucksListQueryHandlerTests()
        {
            _trucksRepositoryMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnNotEmptyCollection_WhithQuery()
        {
            //Arrange
            var trucksCollection = new List<Truck>{
                new Truck("Code", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning },
                new Truck("Code1", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning },
                new Truck("Code2", "Name", "Description") { Id = Guid.NewGuid(), Status = TruckStatus.Returning } };
            var query = new GetTrucksListQuery("Code", TruckStatus.Returning, null, null, 1, 10);
            _trucksRepositoryMock.Setup(x => x.GetTrucksAsync(It.IsAny<string>(), It.IsAny<TruckStatus>(), It.IsAny<string?>(), It.IsAny<string?>(), 1, 10)).ReturnsAsync(trucksCollection);
            var queryHandler = new GetTrucksListQueryHandler(_trucksRepositoryMock.Object);

            //Act
            var result = await queryHandler.Handle(query, default);

            //Assert
            result.Should().BeAssignableTo<ICollection<Truck>>();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(3);
        }
    }
}
