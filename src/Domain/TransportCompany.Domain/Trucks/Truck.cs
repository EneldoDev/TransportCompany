using ErrorOr;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TransportCompany.Domain.Common;
using TransportCompany.Domain.Constants;

namespace TransportCompany.Domain.Trucks
{
    public class Truck : BaseEntity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DefaultValue(TruckStatus.OutOfService)]
        public TruckStatus Status { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public Truck(
            string code,
            string name,
            string? description)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Description = description;
            Status = TruckStatus.OutOfService;
        }

        public ErrorOr<Success> ChangeStatus(TruckStatus newStatus)
        {
            var isValidTransition = TruckStatusTransitions.IsValidTransition(this.Status, newStatus);
            if (!isValidTransition)
            {
                return Error.Validation(ErrorMessages.TruckStatusChange);
            }

            this.Status = newStatus;
            return Result.Success;
        }

        public ErrorOr<Success> UpdateTruck(string code, string name, string description, TruckStatus status)
        {
            var changeStatusResult = ChangeStatus(status);
            if(changeStatusResult.IsError)
            {
                return changeStatusResult;
            }

            this.Code = code;
            this.Name = name;
            this.Description = description;
            return Result.Success;
        }

        public void DeleteTruck()
        {
            this.IsDeleted = true;
        }
    }
}
