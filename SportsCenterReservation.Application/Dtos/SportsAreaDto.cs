using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Dtos;

public class SportsAreaCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public decimal HourlyRate { get; set; }
}

public class SportsAreaReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsActive { get; set; }
}