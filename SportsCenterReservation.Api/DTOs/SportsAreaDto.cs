namespace SportsCenterReservation.Api.DTOs;

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