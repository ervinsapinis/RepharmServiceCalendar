using MediatR;
using Microsoft.AspNetCore.Mvc;
using RepharmServiceCalendar.Commands;
using RepharmServiceCalendar.Models;
using RepharmServiceCalendar.Queries;

namespace RepharmServiceCalendar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAppointments(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAppointmentsQuery(), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] Guid serviceTypeId, [FromQuery] DateTime date, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAvailableSlotsQuery(serviceTypeId, date), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetCustomersQuery(), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("service-types")]
        public async Task<IActionResult> GetServiceTypes(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetServiceTypesQuery(), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentViewModel model, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CreateAppointmentCommand { Model = model }, cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
