
namespace RepharmServiceCalendar.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime TimeSlot { get; set; }
        public Guid ServiceId { get; set; }
        public Guid CustomerId { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Service Service { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
