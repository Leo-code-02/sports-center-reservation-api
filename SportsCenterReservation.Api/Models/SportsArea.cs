namespace SportsCenterReservation.Api.Models
{
    public class SportsArea
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SportType { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
