using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.DTOs;
using System.Net;

namespace RepharmServiceCalendar.Queries
{
    public record GetAppointmentsQuery : IRequest<BaseResponse<List<AppointmentDto>>>
    {
        public Guid ServiceTypeId { get; init; }
    }
    public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, BaseResponse<List<AppointmentDto>>>
    {
        private readonly IMapper _mapper;
        private readonly BackendContext _context;

        public GetAppointmentsQueryHandler(BackendContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<List<AppointmentDto>>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .ThenInclude(s => s.ServiceType)
                .Where(a => a.Service.ServiceTypeId == request.ServiceTypeId)
                .ToListAsync(cancellationToken); 
            
            var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments);

            return new BaseResponse<List<AppointmentDto>>(appointmentDtos, false, null, HttpStatusCode.OK);
        }
    }
}
