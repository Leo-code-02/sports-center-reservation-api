namespace SportsCenterReservation.Api.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int SportsAreaId { get; set; }
        public SportsArea? SportsArea { get; set; }
    }
}
