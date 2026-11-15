using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Dtos;

public class ScheduleCreateDto
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