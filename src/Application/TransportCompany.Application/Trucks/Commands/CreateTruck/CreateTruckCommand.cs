using ErrorOr;
using MediatR;

namespace TransportCompany.Application.Trucks.Commands.CreateTruck
{
    public record CreateTruckCommand(string Code, string Name, string Description) : IRequest<ErrorOr<Guid>>;
}
