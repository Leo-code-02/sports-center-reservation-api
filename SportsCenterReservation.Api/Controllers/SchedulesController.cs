namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSchedules()
    {
        var result = await _scheduleService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchedule(int id)
    {
        var result = await _scheduleService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchedule(ScheduleCreateDto dto)
    {
        var result = await _scheduleService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);

        return CreatedAtAction(nameof(GetSchedule), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, ScheduleCreateDto dto)
    {
        var result = await _scheduleService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var result = await _scheduleService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}