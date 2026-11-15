namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;
using static SportsCenterReservation.Api.DTOs.ReservationDto;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISportsAreaRepository _sportsAreaRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public ReservationsController(
        IReservationRepository reservationRepository,
        IUserRepository userRepository,
        ISportsAreaRepository sportsAreaRepository,
        IScheduleRepository scheduleRepository)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _sportsAreaRepository = sportsAreaRepository;
        _scheduleRepository = scheduleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservations()
    {
        var reservations = await _reservationRepository.GetReservationsWithDetailsAsync();
        var result = reservations.Select(r => new ReservationResponseDto
        {
            Id = r.Id,
            UserId = r.UserId,
            UserName = r.User?.FullName ?? string.Empty,
            SportsAreaId = r.SportsAreaId,
            SportsAreaName = r.SportsArea?.Name ?? string.Empty,
            ScheduleId = r.ScheduleId,
            ReservationDate = r.ReservationDate,
            Status = r.Status,
            TotalAmount = r.TotalAmount
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationResponseDto>> GetReservation(int id)
    {
        var r = await _reservationRepository.GetByIdAsync(id);
        if (r == null) return NotFound();

        return Ok(new ReservationResponseDto
        {
            Id = r.Id,
            UserId = r.UserId,
            SportsAreaId = r.SportsAreaId,
            ScheduleId = r.ScheduleId,
            ReservationDate = r.ReservationDate,
            Status = r.Status,
            TotalAmount = r.TotalAmount
        });
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation(ReservationCreateDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null) return BadRequest("User not found.");

        var area = await _sportsAreaRepository.GetByIdAsync(dto.SportsAreaId);
        if (area == null) return BadRequest("Sports area not found.");

        var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);
        if (schedule == null) return BadRequest("Schedule not found.");

        var available = await _reservationRepository.IsSlotAvailableAsync(
            dto.SportsAreaId, dto.ScheduleId, dto.ReservationDate);

        if (!available)
            return Conflict("The selected sports area and schedule are already reserved for that date.");

        var reservation = new Reservation
        {
            UserId = dto.UserId,
            SportsAreaId = dto.SportsAreaId,
            ScheduleId = dto.ScheduleId,
            ReservationDate = dto.ReservationDate,
            Status = ReservationStatus.Pending,
            TotalAmount = area.HourlyRate
        };

        var created = await _reservationRepository.AddAsync(reservation);

        return CreatedAtAction(nameof(GetReservation), new { id = created.Id }, new ReservationResponseDto
        {
            Id = created.Id,
            UserId = created.UserId,
            UserName = user.FullName,
            SportsAreaId = created.SportsAreaId,
            SportsAreaName = area.Name,
            ScheduleId = created.ScheduleId,
            ReservationDate = created.ReservationDate,
            Status = created.Status,
            TotalAmount = created.TotalAmount
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null) return NotFound();

        reservation.Status = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(reservation);

        return NoContent();
    }
}