namespace RepharmServiceCalendar.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public Guid ServiceTypeId { get; set; }
        //public int ServiceDurationInMinutes { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ServiceType ServiceType { get; set; }
    }
}
