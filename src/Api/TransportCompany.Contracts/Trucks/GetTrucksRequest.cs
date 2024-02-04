using TransportCompany.Domain.Trucks;

namespace TransportCompany.Contracts.Trucks
{
    public record GetTrucksRequest(string? SearchTerm, TruckStatus? Status, string? SortColumn, string? SortOrder, int PageNumber, int ItemsPerPage);
}
