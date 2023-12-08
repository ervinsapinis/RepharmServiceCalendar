using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.Constants;
using RepharmServiceCalendar.DTOs;
using RepharmServiceCalendar.Entities;
using RepharmServiceCalendar.Models;
using System.Net;

namespace RepharmServiceCalendar.Commands
{
    public record CreateAppointmentCommand : IRequest<BaseResponse<AppointmentDto>>
    {
        public AppointmentViewModel Model { get; set; }
    }
        public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, BaseResponse<AppointmentDto>>
        {
            private readonly IMapper _mapper;
            private readonly BackendContext _context;

            public CreateAppointmentCommandHandler(BackendContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<BaseResponse<AppointmentDto>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
            {
                var service = await _context.Services
                                .Include(s => s.ServiceType)
                                .FirstOrDefaultAsync(s => s.Id == request.Model.ServiceId);
                var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.Model.CustomerId);

                if (service == null || !customerExists)
                    return new BaseResponse<AppointmentDto>(null, true, ErrorCodes.InvalidArgument, HttpStatusCode.BadRequest);

                //potenciāla loģika sarežģītākai implementācijai ar dažādiem laika intervāliem priekš katra pakalpojuma.
                if (!IsAppointmentTimeValid(request.Model.TimeSlot, service))
                {
                    return new BaseResponse<AppointmentDto>(null, true, ErrorCodes.InvalidAppointmentTime, HttpStatusCode.BadRequest);
                }

                var appointmentToAdd = _mapper.Map<Appointment>(request.Model);
                appointmentToAdd.DateCreated = DateTime.UtcNow;
                appointmentToAdd.DateUpdated = DateTime.UtcNow;
                appointmentToAdd.Status = AppointmentStatus.Booked;

                await _context.AddAsync(appointmentToAdd);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<AppointmentDto>(appointmentToAdd);

                return new BaseResponse<AppointmentDto>(result, false, null, HttpStatusCode.OK);
            }

            private bool IsAppointmentTimeValid(DateTime timeSlot, Service service)
            {
                var serviceType = service.ServiceType;
                var dayStartTime = new DateTime(timeSlot.Year, timeSlot.Month, timeSlot.Day,
                                                serviceType.StartTime.Hours, serviceType.StartTime.Minutes, 0);
                var dayEndTime = new DateTime(timeSlot.Year, timeSlot.Month, timeSlot.Day,
                                              serviceType.EndTime.Hours, serviceType.EndTime.Minutes, 0);

                if (timeSlot.Minute != 0 || timeSlot < dayStartTime || timeSlot >= dayEndTime)
                {
                    return false;
                }

                var appointmentEnd = timeSlot.AddHours(1);
                var overlappingAppointment = _context.Appointments
                    .Any(a => a.ServiceId == service.Id && a.TimeSlot < appointmentEnd && a.TimeSlot.AddHours(1) > timeSlot);

                return !overlappingAppointment;
            }
        }
}
