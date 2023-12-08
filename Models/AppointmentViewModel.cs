namespace RepharmServiceCalendar.Models
{
    public class AppointmentViewModel
    {
        public DateTime TimeSlot { get; set; }
        public Guid ServiceId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
