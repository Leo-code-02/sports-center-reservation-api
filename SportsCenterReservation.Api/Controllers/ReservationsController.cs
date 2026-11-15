namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        var result = await _reservationService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        var result = await _reservationService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(ReservationCreateDto dto)
    {
        var result = await _reservationService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);

        return CreatedAtAction(nameof(GetReservation), new { id = result.Data!.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var result = await _reservationService.CancelAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}