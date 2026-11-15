using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Dtos;

using SportsCenterReservation.Domain.Entities;

public class ReservationCreateDto
{
    public int UserId { get; set; }
    public int SportsAreaId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime ReservationDate { get; set; }
}

public class ReservationResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int SportsAreaId { get; set; }
    public string SportsAreaName { get; set; } = string.Empty;
    public int ScheduleId { get; set; }
    public DateTime ReservationDate { get; set; }
    public ReservationStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
}