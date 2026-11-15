using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Service;

using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISportsAreaRepository _sportsAreaRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public ReservationService(
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

    public async Task<ServiceResult<IEnumerable<ReservationResponseDto>>> GetAllAsync()
    {
        var reservations = await _reservationRepository.GetReservationsWithDetailsAsync();
        var dtos = reservations.Select(r => new ReservationResponseDto
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
        return ServiceResult<IEnumerable<ReservationResponseDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<ReservationResponseDto>> GetByIdAsync(int id)
    {
        var r = await _reservationRepository.GetByIdAsync(id);
        if (r == null)
            return ServiceResult<ReservationResponseDto>.Fail("Reservation not found.");

        return ServiceResult<ReservationResponseDto>.Ok(new ReservationResponseDto
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

    public async Task<ServiceResult<ReservationResponseDto>> CreateAsync(ReservationCreateDto dto)
    {
        if (dto.ReservationDate.Date < DateTime.UtcNow.Date)
            return ServiceResult<ReservationResponseDto>.Fail("Reservation date cannot be in the past.");

        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            return ServiceResult<ReservationResponseDto>.Fail("User not found.");

        var area = await _sportsAreaRepository.GetByIdAsync(dto.SportsAreaId);
        if (area == null)
            return ServiceResult<ReservationResponseDto>.Fail("Sports area not found.");

        var schedule = await _scheduleRepository.GetByIdAsync(dto.ScheduleId);
        if (schedule == null)
            return ServiceResult<ReservationResponseDto>.Fail("Schedule not found.");

        var available = await _reservationRepository.IsSlotAvailableAsync(
            dto.SportsAreaId, dto.ScheduleId, dto.ReservationDate);

        if (!available)
            return ServiceResult<ReservationResponseDto>.Fail("The selected slot is already reserved.");

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

        return ServiceResult<ReservationResponseDto>.Ok(new ReservationResponseDto
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
        }, "Reservation created.");
    }

    public async Task<ServiceResult<bool>> CancelAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
            return ServiceResult<bool>.Fail("Reservation not found.");

        if (reservation.ReservationDate.Date < DateTime.UtcNow.Date)
            return ServiceResult<bool>.Fail("Cannot cancel past reservations.");

        reservation.Status = ReservationStatus.Cancelled;
        await _reservationRepository.UpdateAsync(reservation);

        return ServiceResult<bool>.Ok(true, "Reservation cancelled.");
    }
}