namespace SportsCenterReservation.Api.DTOs
{
    using SportsCenterReservation.Api.Models;
    public class PaymentDto
    {
        public class PaymentCreateDto
        {
            public int ReservationId { get; set; }
            public decimal Amount { get; set; }
            public PaymentMethod Method { get; set; }
        }

        public class PaymentResponseDto
        {
            public int Id { get; set; }
            public int ReservationId { get; set; }
            public decimal Amount { get; set; }
            public PaymentMethod Method { get; set; }
            public DateTime PaymentDate { get; set; }
        }

        public class ReservationCancellationDto
        {
            public int ReservationId { get; set; }
            public string CancellationReason { get; set; } = string.Empty;
        }
    }
}
