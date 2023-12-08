namespace RepharmServiceCalendar.DTOs
{
    public class ServiceTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public virtual ICollection<ServiceDto> Services { get; set; }
    }
}
