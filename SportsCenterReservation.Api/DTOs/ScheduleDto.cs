namespace SportsCenterReservation.Api.DTOs
{
    public class ScheduleDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SportsAreaId { get; set; }
    }

    public class ScheduleReadDto
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SportsAreaId { get; set; }
    }
}
