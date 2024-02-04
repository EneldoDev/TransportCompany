using ErrorOr;
using MediatR;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Commands.EditTruck
{
    public record EditTruckCommand(Guid Id, string Code, string Name, string Description, TruckStatus status) :IRequest<ErrorOr<Success>>;
}
