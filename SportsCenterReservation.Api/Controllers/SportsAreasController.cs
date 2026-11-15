namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SportsAreasController : ControllerBase
{
    private readonly ISportsAreaRepository _sportsAreaRepository;

    public SportsAreasController(ISportsAreaRepository sportsAreaRepository)
    {
        _sportsAreaRepository = sportsAreaRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SportsAreaReadDto>>> GetSportsAreas()
    {
        var areas = await _sportsAreaRepository.GetAllAsync();
        var result = areas.Select(a => new SportsAreaReadDto
        {
            Id = a.Id,
            Name = a.Name,
            SportType = a.SportType,
            MaxCapacity = a.MaxCapacity,
            HourlyRate = a.HourlyRate,
            IsActive = a.IsActive
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SportsAreaReadDto>> GetSportsArea(int id)
    {
        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null) return NotFound();

        return Ok(new SportsAreaReadDto
        {
            Id = area.Id,
            Name = area.Name,
            SportType = area.SportType,
            MaxCapacity = area.MaxCapacity,
            HourlyRate = area.HourlyRate,
            IsActive = area.IsActive
        });
    }

    [HttpPost]
    public async Task<ActionResult<SportsAreaReadDto>> CreateSportsArea(SportsAreaCreateDto dto)
    {
        var area = new SportsArea
        {
            Name = dto.Name,
            SportType = dto.SportType,
            MaxCapacity = dto.MaxCapacity,
            HourlyRate = dto.HourlyRate
        };

        var created = await _sportsAreaRepository.AddAsync(area);

        var readDto = new SportsAreaReadDto
        {
            Id = created.Id,
            Name = created.Name,
            SportType = created.SportType,
            MaxCapacity = created.MaxCapacity,
            HourlyRate = created.HourlyRate,
            IsActive = created.IsActive
        };

        return CreatedAtAction(nameof(GetSportsArea), new { id = created.Id }, readDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSportsArea(int id, SportsAreaCreateDto dto)
    {
        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null) return NotFound();

        area.Name = dto.Name;
        area.SportType = dto.SportType;
        area.MaxCapacity = dto.MaxCapacity;
        area.HourlyRate = dto.HourlyRate;

        await _sportsAreaRepository.UpdateAsync(area);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSportsArea(int id)
    {
        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null) return NotFound();

        await _sportsAreaRepository.DeleteAsync(area);
        return NoContent();
    }
}