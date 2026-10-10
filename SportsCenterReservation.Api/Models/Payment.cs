namespace SportsCenterReservation.Api.Models
{
    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer
    }
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } = PaymentMethod.CreditCard;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
