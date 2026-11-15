using System;
using System.Collections.Generic;
using System.Text;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Entities;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public class Reservation : BaseEntity
{
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public decimal TotalAmount { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int SportsAreaId { get; set; }
    public SportsArea? SportsArea { get; set; }

    public int ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }
}