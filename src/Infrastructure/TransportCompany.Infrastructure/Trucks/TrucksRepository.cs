using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using TransportCompany.Application.Interfaces;
using TransportCompany.Domain.Trucks;
using TransportCompany.Infrastructure.Common.Persistance;

namespace TransportCompany.Infrastructure.Trucks
{
    public class TrucksRepository(CompanyDbContext _dbContext) : ITrucksRepository
    {
        public async Task AddAsync(Truck truck, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(truck, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Truck?> GetByIdAsync(Guid truckId, CancellationToken cancellationToken)
        {
            return await _dbContext.Trucks.FirstOrDefaultAsync(x => x.Id == truckId && !x.IsDeleted, cancellationToken);
        }

        public async Task UpdateAsync(Truck truck, CancellationToken cancellationToken)
        {
            _dbContext.Update(truck);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> HasUniqueCode(string code, Guid? truckId = null)
        {
            //var query = _dbContext.Trucks.Where(x => !x.IsDeleted && x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            var query = _dbContext.Trucks.Where(x => !x.IsDeleted && x.Code ==code);

            if (truckId.HasValue)
            {
                query = query.Where(x => !x.Id.Equals(truckId));
            }

            return !query.Any();
        }

        public async Task<ICollection<Truck>> GetTrucksAsync(string? searchTerm, TruckStatus? status, string? sortColumn, string? sortOrder, int pageNumber, int itemsPerPage)
        {
            var query = _dbContext.Trucks.AsQueryable();
            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm) || x.Code.Contains(searchTerm) || x.Description.Contains(searchTerm));
            }

            if(status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }

            if(sortOrder?.ToLower()=="desc")
            {
                query = query.OrderByDescending(GetTruckSortColumn(sortColumn));
            }
            else
            {
                query = query.OrderBy(GetTruckSortColumn(sortColumn));
            }

            query = query.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
            return await query.ToArrayAsync();
        }

        private static Expression<Func<Truck, object>> GetTruckSortColumn(string? sortColumn) =>
            sortColumn?.ToLower() switch
            {
                "name" => truck => truck.Name,
                "description" => truck => truck.Description,
                "code" => truck => truck.Code,
                "status" => truck => truck.Status,
                _ => truck => truck.Name
            };
    }
}