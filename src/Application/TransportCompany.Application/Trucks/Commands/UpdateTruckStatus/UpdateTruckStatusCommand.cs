using ErrorOr;
using MediatR;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Commands.UpdateTruckStatus
{
    public record UpdateTruckStatusCommand(Guid TruckId, TruckStatus Status) : IRequest<ErrorOr<Success>>;
}
