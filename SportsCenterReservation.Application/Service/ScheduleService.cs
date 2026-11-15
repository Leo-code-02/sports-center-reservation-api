using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Service;

using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISportsAreaRepository _sportsAreaRepository;

    public ScheduleService(IScheduleRepository scheduleRepository, ISportsAreaRepository sportsAreaRepository)
    {
        _scheduleRepository = scheduleRepository;
        _sportsAreaRepository = sportsAreaRepository;
    }

    public async Task<ServiceResult<IEnumerable<ScheduleReadDto>>> GetAllAsync()
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        var dtos = schedules.Select(s => new ScheduleReadDto
        {
            Id = s.Id,
            DayOfWeek = s.DayOfWeek,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            SportsAreaId = s.SportsAreaId
        });
        return ServiceResult<IEnumerable<ScheduleReadDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<ScheduleReadDto>> GetByIdAsync(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return ServiceResult<ScheduleReadDto>.Fail("Schedule not found.");

        return ServiceResult<ScheduleReadDto>.Ok(new ScheduleReadDto
        {
            Id = schedule.Id,
            DayOfWeek = schedule.DayOfWeek,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SportsAreaId = schedule.SportsAreaId
        });
    }

    public async Task<ServiceResult<ScheduleReadDto>> CreateAsync(ScheduleCreateDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            return ServiceResult<ScheduleReadDto>.Fail("Start time must be earlier than end time.");

        if (!await _sportsAreaRepository.ExistsAsync(a => a.Id == dto.SportsAreaId))
            return ServiceResult<ScheduleReadDto>.Fail("Sports area does not exist.");

        var schedule = new Schedule
        {
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            SportsAreaId = dto.SportsAreaId
        };

        var created = await _scheduleRepository.AddAsync(schedule);

        return ServiceResult<ScheduleReadDto>.Ok(new ScheduleReadDto
        {
            Id = created.Id,
            DayOfWeek = created.DayOfWeek,
            StartTime = created.StartTime,
            EndTime = created.EndTime,
            SportsAreaId = created.SportsAreaId
        }, "Schedule created.");
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, ScheduleCreateDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            return ServiceResult<bool>.Fail("Start time must be earlier than end time.");

        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return ServiceResult<bool>.Fail("Schedule not found.");

        if (!await _sportsAreaRepository.ExistsAsync(a => a.Id == dto.SportsAreaId))
            return ServiceResult<bool>.Fail("Sports area does not exist.");

        schedule.DayOfWeek = dto.DayOfWeek;
        schedule.StartTime = dto.StartTime;
        schedule.EndTime = dto.EndTime;
        schedule.SportsAreaId = dto.SportsAreaId;

        await _scheduleRepository.UpdateAsync(schedule);
        return ServiceResult<bool>.Ok(true, "Schedule updated.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return ServiceResult<bool>.Fail("Schedule not found.");

        await _scheduleRepository.DeleteAsync(schedule);
        return ServiceResult<bool>.Ok(true, "Schedule deleted.");
    }
}