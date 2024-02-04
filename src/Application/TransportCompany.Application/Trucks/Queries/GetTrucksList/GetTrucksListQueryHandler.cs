using MediatR;
using TransportCompany.Application.Interfaces;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Queries.GetTrucksList
{
    public class GetTrucksListQueryHandler(ITrucksRepository _truckRepository) : IRequestHandler<GetTrucksListQuery, ICollection<Truck>>
    {
        public async Task<ICollection<Truck>> Handle(GetTrucksListQuery request, CancellationToken cancellationToken)
        {
            return await _truckRepository.GetTrucksAsync(request.SearchTerm, request.Status, request.SortColumn, request.SortOrder, request.PageNumber, request.ItemsPerPage);
        }
    }
}
