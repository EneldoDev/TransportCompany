using ErrorOr;
using MediatR;
using TransportCompany.Application.Interfaces;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Trucks.Queries.GetTruck
{
    public class GetTruckQueryHandler(ITrucksRepository _truckRepository) : IRequestHandler<GetTruckQuery, ErrorOr<Truck>>
    {
        public async Task<ErrorOr<Truck>> Handle(GetTruckQuery request, CancellationToken cancellationToken)
        {
            var truck = await _truckRepository.GetByIdAsync(request.TruckId, cancellationToken);
            if (truck == null)
            {
                return Error.NotFound();
            }

            return truck;
        }
    }
}
