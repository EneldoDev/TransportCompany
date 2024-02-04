using MediatR;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Queries.GetTrucksList
{
    public record GetTrucksListQuery(string? SearchTerm, TruckStatus? Status, string? SortColumn, string? SortOrder, int PageNumber, int ItemsPerPage) : IRequest<ICollection<Truck>>;
}
