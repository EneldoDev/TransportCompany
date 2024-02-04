using ErrorOr;
using MediatR;
using TransportCompany.Application.Constants;
using TransportCompany.Application.Interfaces;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Commands.CreateTruck
{
    public class CreateTruckCommandHandler(ITrucksRepository _trucksRepository) : IRequestHandler<CreateTruckCommand, ErrorOr<Guid>>
    {
        public async Task<ErrorOr<Guid>> Handle(CreateTruckCommand request, CancellationToken cancellationToken)
        {
            var hasUniqueCode = await _trucksRepository.HasUniqueCode(request.Code);
            if(!hasUniqueCode)
            {
                return Error.Conflict(ErrorMessages.TruckCodeContlict);
            }

            var truck = new Truck(request.Code, request.Name, request.Description);
            await _trucksRepository.AddAsync(truck, cancellationToken);
            return truck.Id;
        }
    }
}
