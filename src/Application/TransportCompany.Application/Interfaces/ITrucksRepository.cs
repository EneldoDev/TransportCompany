using TransportCompany.Domain.Trucks;

namespace TransportCompany.Application.Interfaces
{
    public interface ITrucksRepository
    {
        Task AddAsync(Truck truck, CancellationToken cancellationToken);
        Task<Truck?> GetByIdAsync(Guid truckId, CancellationToken cancellationToken);
        Task UpdateAsync(Truck truck, CancellationToken cancellationToken);
        Task<bool> HasUniqueCode(string code, Guid? truckId = null);
        Task<ICollection<Truck>> GetTrucksAsync(string? searchTerm, TruckStatus? status, string? sortColumn, string? sortOrder, int pageNumber, int itemsPerPage);
    }
}
