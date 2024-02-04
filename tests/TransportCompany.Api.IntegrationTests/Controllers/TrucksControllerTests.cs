using FluentAssertions;
using System.Net.Http.Json;
using TransportCompany.Api.IntegrationTests.Common;
using TransportCompany.Contracts.Trucks;
using TransportCompany.Domain.Trucks;
using TransportCompany.TestCommon.Constants;

namespace TransportCompany.Api.IntegrationTests.Controllers
{
    public class TrucksControllerTests
    {
        private readonly HttpClient _httpClient;

        public TrucksControllerTests()
        {
            var applicationFactory = new TestWebAppFactory();
            _httpClient = applicationFactory.CreateClient();
        }

        [Fact]
        public async Task CreateTruck_ShouldCreateTruck_WithProperDetails()
        {
            var truckId = await CreateDefaultTruck();
            var truckDetails = await GetTruck(truckId);

            //Assert
            truckDetails.Should().NotBeNull();
            truckDetails.Id.Should().Be(truckId);
            truckDetails.Name.Should().Be(TruckConstants.TruckName);
            truckDetails.Description.Should().Be(TruckConstants.TruckDescription);
            truckDetails.Code.Should().Be(TruckConstants.TruckCode);
            truckDetails.Status.Should().Be(TruckStatus.OutOfService);
            truckDetails.IsDeleted.Should().Be(false);
        }

        [Fact]
        public async Task CreateTruck_ShouldNotAllowToCreateTruck_WithExistingCode()
        {
            // Arrange
            var url = "/api/Trucks";
            var truckRequest = new CreateTruckRequest(TruckConstants.TruckCode, TruckConstants.TruckName, TruckConstants.TruckDescription);
            await _httpClient.PostAsJsonAsync(url, truckRequest);

            // Act
            var result = await _httpClient.PostAsJsonAsync(url, truckRequest);
            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().HaveFlag(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task EditTruck_ShouldUpdateTruck_WithNewProperties()
        {
            //Arrange
            var truckId = await CreateDefaultTruck();
            var editTruckUrl = $"/api/Trucks/{truckId}/EditTruck";
            var editTruckRequest = new EditTruckRequest("EditedCode", "EditedName", "EditedDescription", TruckStatus.AtJob);
            //Act
            var result = await _httpClient.PostAsJsonAsync(editTruckUrl, editTruckRequest);

            //Assert
            result.EnsureSuccessStatusCode();
            var truckDetails = await GetTruck(truckId);

            truckDetails.Should().NotBeNull();
            truckDetails.Id.Should().Be(truckId);
            truckDetails.Name.Should().Be("EditedName");
            truckDetails.Description.Should().Be("EditedDescription");
            truckDetails.Code.Should().Be("EditedCode");
            truckDetails.Status.Should().Be(TruckStatus.AtJob);
            truckDetails.IsDeleted.Should().Be(false);
        }


        [Fact]
        public async Task UpdateTruckStatus_ShouldChangeTruckStatus_WhenProperFlow()
        {
            //Arrange
            var truckId = await CreateDefaultTruck();
            var updateTruckStatusUrl = $"/api/Trucks/{truckId}/UpdateTruckStatus";
            var updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);

            //AtJob -> OutOfService -> AtJob -> Returning
            //Act
            var updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.OutOfService);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Returning);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //Returning -> OutOfService -> Returning -> Loading
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.OutOfService);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Returning);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Loading);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //Loading -> OutOfService -> Loading -> ToJob
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.OutOfService);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Loading);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.ToJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //ToJob -> OutOfService -> ToJob -> AtJob
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.OutOfService);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.ToJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();
            var truck = await GetTruck(truckId);
            truck.Status.Should().HaveFlag(TruckStatus.AtJob);
        }

        [Fact]
        public async Task UpdateTruckStatus_ShouldNotAllowToChangeTruckStatus_WhenImproperProperFlow()
        {
            //Arrange
            var truckId = await CreateDefaultTruck();
            var updateTruckStatusUrl = $"/api/Trucks/{truckId}/UpdateTruckStatus";
            var updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);
            //Act
            var updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.EnsureSuccessStatusCode();

            //AtJob allows to transit to Returning
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Loading);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.ToJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Returning allows to transit to Loading
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Returning);
            await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.ToJob);

            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Loading allows to transit to ToJob
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Loading);
            await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Returning);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.AtJob);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //ToJob allows transint to AtJob
            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.ToJob);
            await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Returning);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            //Arrange
            updateTruckStatusRequest = new UpdateTruckStatusRequest(TruckStatus.Loading);
            //Act
            updateResponse = await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
            //Assert
            updateResponse.IsSuccessStatusCode.Should().BeFalse();

            var truck = await GetTruck(truckId);
            truck.Status.Should().HaveFlag(TruckStatus.ToJob);
        }

        [Fact]
        public async Task DeleteTruck_ShouldDeleteTruck_WhenTruckIsNotDeleted()
        {
            //Arrange
            var truckId = await CreateDefaultTruck();
            var deleteUrl = $"/api/Trucks/{truckId}/DeleteTruck";

            //Act
            var deleteResult = await _httpClient.DeleteAsync(deleteUrl);

            //Assert
            deleteResult.EnsureSuccessStatusCode();
            var getUrl = $"/api/Trucks/{truckId}";
            //var getTruckResult = await _httpClient.GetFromJsonAsync(getUrl, typeof(ErrorOr<Truck>));
            var getTruckResult = await _httpClient.GetAsync(getUrl);
            getTruckResult.IsSuccessStatusCode.Should().BeFalse();
            getTruckResult.StatusCode.Should().HaveFlag(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteTruck_ShouldReturnNotFound_WhenTryingToDeleteDeletedTruck()
        {
            //Arrange
            var truckId = await CreateDefaultTruck();
            var deleteUrl = $"/api/Trucks/{truckId}/DeleteTruck";
            var deleteResult = await _httpClient.DeleteAsync(deleteUrl);

            //Act
            deleteResult = await _httpClient.DeleteAsync(deleteUrl);
            //Assert
            deleteResult.IsSuccessStatusCode.Should().BeFalse();
            deleteResult.StatusCode.Should().HaveFlag(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetTrucks_ShouldReturnCollection_WithDifferentFilters()
        {
            //Arrange
            await CreateCollectionOfTrucks();
            //Act
            //https://localhost:7043/api/Trucks/GetTrucks?SearchTerm=tern&Status=2&SortColumn=Code&SortOrder=desc&PageNumber=1&ItemsPerPage=2'
            //Without search term and status
            var getUrl = "/api/Trucks/GetTrucks?PageNumber=1&ItemsPerPage=20";
            var result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            result.Should().HaveCount(10);

            //Act
            //Search by term without status
            getUrl = "/api/Trucks/GetTrucks?SearchTerm=TruckName_1&PageNumber=1&ItemsPerPage=20";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            //Assert
            result.Should().HaveCount(5);

            //Act
            //With status, without search term
            getUrl = "/api/Trucks/GetTrucks?Status=2&PageNumber=1&ItemsPerPage=20";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            //Assert
            result.Should().HaveCount(2);

            //Act
            //With status and search term
            getUrl = "/api/Trucks/GetTrucks?SearchTerm=TruckName_1&Status=2&PageNumber=1&ItemsPerPage=20";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            //Assert
            result.Should().HaveCount(1);

            //With pagination

            //Act
            //Without search term and status
            getUrl = "/api/Trucks/GetTrucks?PageNumber=2&ItemsPerPage=3";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            result.Should().HaveCount(3);

            //Act
            //Search by term without status
            getUrl = "/api/Trucks/GetTrucks?SearchTerm=TruckName_1&PageNumber=2&ItemsPerPage=3";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            //Assert
            result.Should().HaveCount(2);

            //Act
            //With status, without search term
            getUrl = "/api/Trucks/GetTrucks?Status=2&PageNumber=2&ItemsPerPage=1";
            result = await _httpClient.GetFromJsonAsync<List<Truck>>(getUrl);
            //Assert
            result.Should().HaveCount(1);
        }

        private async Task<Guid> CreateDefaultTruck()
        {
            // Arrange
            var url = "/api/Trucks";
            var truckRequest = new CreateTruckRequest(TruckConstants.TruckCode, TruckConstants.TruckName, TruckConstants.TruckDescription);

            // Act
            var result = await _httpClient.PostAsJsonAsync(url, truckRequest);

            // Assert
            result.EnsureSuccessStatusCode();
            var truckId = await result.Content.ReadFromJsonAsync<Guid>();
            truckId.Should().NotBeEmpty();
            return truckId;
        }

        private async Task CreateCollectionOfTrucks()
        {
            var truckStatuses = new []{ TruckStatus.OutOfService, TruckStatus.Loading, TruckStatus.ToJob, TruckStatus.AtJob, TruckStatus.Returning };
            var url = "/api/Trucks";
            for(var i=0; i< 10; i++)
            {
                var truckRequest = new CreateTruckRequest($"TruckCode_{i}_{TruckConstants.TruckCode}", $"TruckName_{i%2}_{TruckConstants.TruckName}", $"TruckDescription_{i%4}_TruckConstants.TruckDescription");
                var result = await _httpClient.PostAsJsonAsync(url, truckRequest);
                var truckId = await result.Content.ReadFromJsonAsync<Guid>();
                if(i%5 !=0)
                {
                    var updateTruckStatusUrl = $"/api/Trucks/{truckId}/UpdateTruckStatus";
                    var updateTruckStatusRequest = new UpdateTruckStatusRequest(truckStatuses[i%5]);
                    await _httpClient.PostAsJsonAsync(updateTruckStatusUrl, updateTruckStatusRequest);
                }
            }
        }

        private async Task<Truck> GetTruck(Guid truckId)
        {
            //Arrange
            var getUrl = $"/api/Trucks/{truckId}";

            //Act
            var getTruckResult = await _httpClient.GetAsync(getUrl);

            //Assert
            getTruckResult.EnsureSuccessStatusCode();
            var truck = await getTruckResult.Content.ReadFromJsonAsync<Truck>();
            truck.Should().NotBeNull();
            return truck;
        }
    }
}
