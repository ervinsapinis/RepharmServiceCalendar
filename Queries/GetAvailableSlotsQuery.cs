using MediatR;
using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.Constants;
using System.Net;

namespace RepharmServiceCalendar.Queries
{
    public record GetAvailableSlotsQuery(Guid ServiceTypeId, DateTime Date) : IRequest<BaseResponse<List<DateTime>>>;

    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, BaseResponse<List<DateTime>>>
    {
        private readonly BackendContext _context;

        public GetAvailableSlotsQueryHandler(BackendContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<DateTime>>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var serviceType = await _context.ServiceTypes
                .FirstOrDefaultAsync(st => st.Id == request.ServiceTypeId, cancellationToken);

            if (serviceType == null)
                return new BaseResponse<List<DateTime>>(null, true, ErrorCodes.ServiceTypeNotFound, HttpStatusCode.NotFound);

            var date = request.Date.Date;
            var startTime = new DateTime(date.Year, date.Month, date.Day, serviceType.StartTime.Hours, serviceType.StartTime.Minutes, 0);
            var endTime = new DateTime(date.Year, date.Month, date.Day, serviceType.EndTime.Hours, serviceType.EndTime.Minutes, 0);

            var services = await _context.Services
                .Where(s => s.ServiceTypeId == request.ServiceTypeId)
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);

            var availableSlots = new List<DateTime>();
            for (var time = startTime; time < endTime; time = time.AddHours(1)) //primitīva implementācijas ar 1 stundas intervāliem priekš pakalpojuma apmeklējuma.
            {
                var isSlotAvailable = !await _context.Appointments
                    .AnyAsync(a => services.Contains(a.ServiceId) && a.TimeSlot == time, cancellationToken);

                if (isSlotAvailable)
                    availableSlots.Add(time);
            }

            return new BaseResponse<List<DateTime>>(availableSlots, false, null, HttpStatusCode.OK);
        }
    }
}
