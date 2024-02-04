using ErrorOr;
using MediatR;
using TransportCompany.Application.Interfaces;

namespace TransportCompany.Application.Trucks.Commands.DeleteTruck
{
    public class DeleteTruckCommandHandler(ITrucksRepository _truckRepository) : IRequestHandler<DeleteTruckCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(DeleteTruckCommand request, CancellationToken cancellationToken)
        {
            var truckToDelete = await _truckRepository.GetByIdAsync(request.TruckId, cancellationToken);
            if(truckToDelete == null)
            {
                return Error.NotFound();
            }

            truckToDelete.DeleteTruck();
            await _truckRepository.UpdateAsync(truckToDelete, cancellationToken);
            return Result.Success;
        }
    }
}
