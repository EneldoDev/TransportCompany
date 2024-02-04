using TransportCompany.Domain.Trucks;

namespace TransportCompany.Contracts.Trucks
{
    public record CreateTruckRequest(string Code, string Name, string Description);
}
