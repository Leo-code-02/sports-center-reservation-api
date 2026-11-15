using System;
using System.Collections.Generic;
using System.Text;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Entities;



public class Schedule : BaseEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int SportsAreaId { get; set; }
    public SportsArea? SportsArea { get; set; }
}