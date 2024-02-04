using ErrorOr;
using MediatR;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Queries.GetTruck
{
    public record GetTruckQuery(Guid TruckId) : IRequest<ErrorOr<Truck>>;
}
