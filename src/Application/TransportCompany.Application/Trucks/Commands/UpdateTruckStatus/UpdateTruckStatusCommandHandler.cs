using ErrorOr;
using MediatR;
using TransportCompany.Application.Interfaces;

namespace TransportCompany.Application.Trucks.Commands.UpdateTruckStatus
{
    public class UpdateTruckStatusCommandHandler(ITrucksRepository _trucksRepository) : IRequestHandler<UpdateTruckStatusCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(UpdateTruckStatusCommand request, CancellationToken cancellationToken)
        {
            var truck = await _trucksRepository.GetByIdAsync(request.TruckId, cancellationToken);

            if (truck == null)
            {
                return Error.NotFound();
            }

            var result = truck.ChangeStatus(request.Status);
            if (result.IsError)
            {
                return result.Errors;
            }

            await _trucksRepository.UpdateAsync(truck, cancellationToken);
            return Result.Success;
        }
    }
}
