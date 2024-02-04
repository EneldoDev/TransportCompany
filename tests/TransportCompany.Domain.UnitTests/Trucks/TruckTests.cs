using FluentAssertions;
using TransportCompany.Domain.Trucks;
using TransportCompany.TestCommon.Constants;

namespace TransportCompanu.Domain.UnitTests.Trucks
{
    public class TruckTests
    {
        [Fact]
        public void CreateTruck_WhenCreated_ShouldHaveStatusAndIsDeletedDefaultValues()
        {
            //Act
            var truck = TruckFactory.CreateTruck();

            //Asset
            truck.IsDeleted.Should().BeFalse();
            truck.Status.Should().HaveFlag(TruckStatus.OutOfService);
        }

        [Fact]
        public void ChangeStatus_WhenOutOfService_ShouldAllowSetAnyOtherStatus()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();

            //Act
            var result = truck.ChangeStatus(TruckStatus.Returning);

            //Assert
            result.IsError.Should().BeFalse();
            truck.Status.Should().HaveFlag(TruckStatus.Returning);
        }

        [Fact]
        public void ChangeStatus_WhenStatusFlowInCorrectOrder_ShouldSetProperStatus()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();
            truck.ChangeStatus(TruckStatus.Loading);
            truck.ChangeStatus(TruckStatus.ToJob);
            truck.ChangeStatus(TruckStatus.AtJob);
            truck.ChangeStatus(TruckStatus.Returning);
            truck.ChangeStatus(TruckStatus.Loading);

            //Act
            var result = truck.ChangeStatus(TruckStatus.ToJob);

            //Assert
            result.IsError.Should().BeFalse();
            truck.Status.Should().HaveFlag(TruckStatus.ToJob);
        }

        [Fact]
        public void ChangeStatus_WhenStatusFlowInIncorrectOrder_ShouldNotAllowChangeStatus()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();
            truck.ChangeStatus(TruckStatus.Loading);
            truck.ChangeStatus(TruckStatus.ToJob);
            truck.ChangeStatus(TruckStatus.Returning);
            truck.ChangeStatus(TruckStatus.ToJob);
            truck.ChangeStatus(TruckStatus.Loading);

            //Act
            var result = truck.ChangeStatus(TruckStatus.ToJob);

            //Assert
            result.IsError.Should().BeTrue();
            truck.Status.Should().HaveFlag(TruckStatus.ToJob);
        }

        [Fact]
        public void ChangeStatus_WhetStatusChangedToOutOfOrder_ShouldAllowChangeStatus()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();
            truck.ChangeStatus(TruckStatus.Loading);
            truck.ChangeStatus(TruckStatus.ToJob);

            //Act
            var result = truck.ChangeStatus(TruckStatus.OutOfService);

            //Assert
            result.IsError.Should().BeFalse();
            truck.Status.Should().HaveFlag(TruckStatus.OutOfService);
        }

        [Fact]
        public void DeleteTruck_WhenIsntDeleted_ShouldSetIsDeletedFlag()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();

            //Act
            truck.DeleteTruck();

            //Asset
            truck.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void UpdateTruck_WhenExecuted_ShouldChangeTruckProperties()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();

            //Act
            var result = truck.UpdateTruck("newCode", "newName", null, TruckStatus.Loading);

            //Assert
            result.IsError.Should().BeFalse();
            truck.IsDeleted.Should().BeFalse();
            truck.Code.Should().BeEquivalentTo("newCode");
            truck.Name.Should().BeEquivalentTo("newName");
            truck.Description.Should().BeNull();
            truck.Status.Should().HaveFlag(TruckStatus.Loading);
        }

        [Fact]
        public void UpdateTruck_WhenExecutedWithWrongStatus_ShouldNotChangeTruckProperties()
        {
            //Arrange
            var truck = TruckFactory.CreateTruck();
            truck.ChangeStatus(TruckStatus.AtJob);
            //Act
            var result = truck.UpdateTruck("newCode", "newName", null, TruckStatus.Loading);

            //Assert
            result.IsError.Should().BeTrue();
            truck.IsDeleted.Should().BeFalse();
            truck.Code.Should().BeEquivalentTo(TruckConstants.TruckCode);
            truck.Name.Should().BeEquivalentTo(TruckConstants.TruckName);
            truck.Description.Should().BeEquivalentTo(TruckConstants.TruckDescription);
            truck.Status.Should().HaveFlag(TruckStatus.Loading);
        }
    }
}
