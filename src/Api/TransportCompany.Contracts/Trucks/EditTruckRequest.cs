using TransportCompany.Domain.Trucks;

namespace TransportCompany.Contracts.Trucks
{
    public record EditTruckRequest(string Code, string Name, string Description, TruckStatus Status);
}
