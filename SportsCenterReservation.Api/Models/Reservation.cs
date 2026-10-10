namespace SportsCenterReservation.Api.Models
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int SportsAreaId { get; set; }
        public SportsArea? SportsArea { get; set; }

        public int ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
