using ErrorOr;
using MediatR;

namespace TransportCompany.Application.Trucks.Commands.DeleteTruck
{
    public record DeleteTruckCommand(Guid TruckId) : IRequest<ErrorOr<Success>>;
}
