namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Api.Data;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class SportsAreasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SportsAreasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SportsAreaReadDto>>> GetSportsAreas()
    {
        var areas = await _context.SportsAreas
            .Select(a => new SportsAreaReadDto
            {
                Id = a.Id,
                Name = a.Name,
                SportType = a.SportType,
                MaxCapacity = a.MaxCapacity,
                HourlyRate = a.HourlyRate,
                IsActive = a.IsActive
            })
            .ToListAsync();

        return Ok(areas);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<SportsAreaReadDto>> GetSportsArea(int id)
    {
        var area = await _context.SportsAreas.FindAsync(id);

        if (area == null)
        {
            return NotFound();
        }

        var dto = new SportsAreaReadDto
        {
            Id = area.Id,
            Name = area.Name,
            SportType = area.SportType,
            MaxCapacity = area.MaxCapacity,
            HourlyRate = area.HourlyRate,
            IsActive = area.IsActive
        };

        return Ok(dto);
    }


    [HttpPost]
    public async Task<ActionResult<SportsAreaReadDto>> CreateSportsArea(SportsAreaCreateDto dto)
    {
        var area = new SportsArea
        {
            Name = dto.Name,
            SportType = dto.SportType,
            MaxCapacity = dto.MaxCapacity,
            HourlyRate = dto.HourlyRate,
            IsActive = true
        };

        _context.SportsAreas.Add(area);
        await _context.SaveChangesAsync();

        var readDto = new SportsAreaReadDto
        {
            Id = area.Id,
            Name = area.Name,
            SportType = area.SportType,
            MaxCapacity = area.MaxCapacity,
            HourlyRate = area.HourlyRate,
            IsActive = area.IsActive
        };

        return CreatedAtAction(nameof(GetSportsArea), new { id = area.Id }, readDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSportsArea(int id, SportsAreaCreateDto dto)
    {
        var area = await _context.SportsAreas.FindAsync(id);

        if (area == null)
        {
            return NotFound();
        }

        area.Name = dto.Name;
        area.SportType = dto.SportType;
        area.MaxCapacity = dto.MaxCapacity;
        area.HourlyRate = dto.HourlyRate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSportsArea(int id)
    {
        var area = await _context.SportsAreas.FindAsync(id);

        if (area == null)
        {
            return NotFound();
        }

        _context.SportsAreas.Remove(area);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}