namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsCenterReservation.Api.Data;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedules()
    {
        var schedules = await _context.Schedules
            .Select(s => new ScheduleReadDto
            {
                Id = s.Id,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                SportsAreaId = s.SportsAreaId
            })
            .ToListAsync();

        return Ok(schedules);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleReadDto>> GetSchedule(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        var dto = new ScheduleReadDto
        {
            Id = schedule.Id,
            DayOfWeek = schedule.DayOfWeek,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SportsAreaId = schedule.SportsAreaId
        };

        return Ok(dto);
    }


    [HttpPost]
    public async Task<ActionResult<ScheduleReadDto>> CreateSchedule(ScheduleCreateDto dto)
    {

        var areaExists = await _context.SportsAreas.AnyAsync(a => a.Id == dto.SportsAreaId);
        if (!areaExists)
        {
            return BadRequest("El área deportiva especificada no existe.");
        }


        if (dto.StartTime >= dto.EndTime)
        {
            return BadRequest("La hora de inicio debe ser anterior a la hora de fin.");
        }

        var schedule = new Schedule
        {
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            SportsAreaId = dto.SportsAreaId
        };

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        var readDto = new ScheduleReadDto
        {
            Id = schedule.Id,
            DayOfWeek = schedule.DayOfWeek,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SportsAreaId = schedule.SportsAreaId
        };

        return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, readDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, ScheduleCreateDto dto)
    {
        var schedule = await _context.Schedules.FindAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        var areaExists = await _context.SportsAreas.AnyAsync(a => a.Id == dto.SportsAreaId);
        if (!areaExists)
        {
            return BadRequest("El área deportiva especificada no existe.");
        }

        if (dto.StartTime >= dto.EndTime)
        {
            return BadRequest("La hora de inicio debe ser anterior a la hora de fin.");
        }

        schedule.DayOfWeek = dto.DayOfWeek;
        schedule.StartTime = dto.StartTime;
        schedule.EndTime = dto.EndTime;
        schedule.SportsAreaId = dto.SportsAreaId;

        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}