namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISportsAreaRepository _sportsAreaRepository;

    public SchedulesController(IScheduleRepository scheduleRepository, ISportsAreaRepository sportsAreaRepository)
    {
        _scheduleRepository = scheduleRepository;
        _sportsAreaRepository = sportsAreaRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleReadDto>>> GetSchedules()
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        var result = schedules.Select(s => new ScheduleReadDto
        {
            Id = s.Id,
            DayOfWeek = s.DayOfWeek,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            SportsAreaId = s.SportsAreaId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleReadDto>> GetSchedule(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null) return NotFound();

        return Ok(new ScheduleReadDto
        {
            Id = schedule.Id,
            DayOfWeek = schedule.DayOfWeek,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SportsAreaId = schedule.SportsAreaId
        });
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleReadDto>> CreateSchedule(ScheduleCreateDto dto)
    {
        if (!await _sportsAreaRepository.ExistsAsync(a => a.Id == dto.SportsAreaId))
            return BadRequest("Sports area does not exist.");

        if (dto.StartTime >= dto.EndTime)
            return BadRequest("Start time must be earlier than end time.");

        var schedule = new Schedule
        {
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            SportsAreaId = dto.SportsAreaId
        };

        var created = await _scheduleRepository.AddAsync(schedule);

        return CreatedAtAction(nameof(GetSchedule), new { id = created.Id }, new ScheduleReadDto
        {
            Id = created.Id,
            DayOfWeek = created.DayOfWeek,
            StartTime = created.StartTime,
            EndTime = created.EndTime,
            SportsAreaId = created.SportsAreaId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, ScheduleCreateDto dto)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null) return NotFound();

        if (!await _sportsAreaRepository.ExistsAsync(a => a.Id == dto.SportsAreaId))
            return BadRequest("Sports area does not exist.");

        if (dto.StartTime >= dto.EndTime)
            return BadRequest("Start time must be earlier than end time.");

        schedule.DayOfWeek = dto.DayOfWeek;
        schedule.StartTime = dto.StartTime;
        schedule.EndTime = dto.EndTime;
        schedule.SportsAreaId = dto.SportsAreaId;

        await _scheduleRepository.UpdateAsync(schedule);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null) return NotFound();

        await _scheduleRepository.DeleteAsync(schedule);
        return NoContent();
    }
}