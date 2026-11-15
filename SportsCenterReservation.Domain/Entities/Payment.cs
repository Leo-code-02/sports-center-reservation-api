using System;
using System.Collections.Generic;
using System.Text;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Entities;

public enum PaymentMethod
{
    Cash,
    CreditCard,
    DebitCard,
    BankTransfer
}

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; } = PaymentMethod.CreditCard;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
}