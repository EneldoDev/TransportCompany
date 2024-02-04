using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransportCompany.Application.Trucks.Commands.CreateTruck;
using TransportCompany.Application.Trucks.Commands.UpdateTruckStatus;
using TransportCompany.Contracts.Trucks;
using TransportCompany.Application.Trucks.Commands.EditTruck;
using TransportCompany.Application.Trucks.Commands.DeleteTruck;
using TransportCompany.Application.Trucks.Queries.GetTruck;
using TransportCompany.Application.Trucks.Queries.GetTrucksList;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController(IMediator _mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTruck(CreateTruckRequest createTruckRequest)
        {
            var command = new CreateTruckCommand(createTruckRequest.Code, createTruckRequest.Name, createTruckRequest.Description);
            var result = await _mediator.Send(command);
            return result.Match(createResult => Ok(result.Value), Problem);
        }

        [HttpPost]
        [Route("{truckId:guid}/[action]")]
        public async Task<IActionResult> UpdateTruckStatus(Guid truckId, UpdateTruckStatusRequest updateTruckStatusRequest)
        {
            var command = new UpdateTruckStatusCommand(truckId, updateTruckStatusRequest.Status);
            var result = await _mediator.Send(command);
            return result.Match(truckStatus =>
            NoContent(), Problem);
        }

        [HttpPost]
        [Route("{truckId:guid}/[action]")]
        public async Task<IActionResult> EditTruck(Guid truckId, EditTruckRequest request)
        {
            var command = new EditTruckCommand(truckId, request.Code, request.Name, request.Description, request.Status);
            var result = await _mediator.Send(command);
            return result.Match(editStatus =>
            NoContent(), Problem);
        }

        [HttpDelete]
        [Route("{truckId:guid}/[action]")]
        public async Task<IActionResult> DeleteTruck(Guid truckId)
        {
            var command = new DeleteTruckCommand(truckId);
            var result = await _mediator.Send(command);
            return result.Match(editStatus =>
                NoContent(), Problem);

        }

        [HttpGet]
        [Route("{truckId:guid}")]
        public async Task<IActionResult> GetTruck(Guid truckId)
        {
            var query = new GetTruckQuery(truckId);
            var result = await _mediator.Send(query);
            return result.Match(truckResult =>
                Ok(result.Value), Problem);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetTrucks([FromQuery] GetTrucksRequest request)
        {
            var query = new GetTrucksListQuery(request.SearchTerm, request.Status, request.SortColumn, request.SortOrder, request.PageNumber, request.ItemsPerPage);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
