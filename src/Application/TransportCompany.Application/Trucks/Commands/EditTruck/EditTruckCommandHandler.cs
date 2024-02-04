using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportCompany.Application.Constants;
using TransportCompany.Application.Interfaces;

namespace TransportCompany.Application.Trucks.Commands.EditTruck
{
    public class EditTruckCommandHandler(ITrucksRepository _trucksRepository) : IRequestHandler<EditTruckCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(EditTruckCommand request, CancellationToken cancellationToken)
        {
            var hasUniqueCode = await _trucksRepository.HasUniqueCode(request.Code, request.Id);
            if (!hasUniqueCode)
            {
                return Error.Conflict(ErrorMessages.TruckCodeContlict);
            }

            var truck = await _trucksRepository.GetByIdAsync(request.Id, cancellationToken);
            if (truck == null)
            {
                return Error.NotFound();
            }

            truck.UpdateTruck(request.Code, request.Name, request.Description, request.status);
            await _trucksRepository.UpdateAsync(truck, cancellationToken);
            return Result.Success;
        }
    }
}
