using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Service;

using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

public class SportsAreaService : ISportsAreaService
{
    private readonly ISportsAreaRepository _sportsAreaRepository;

    public SportsAreaService(ISportsAreaRepository sportsAreaRepository)
    {
        _sportsAreaRepository = sportsAreaRepository;
    }

    public async Task<ServiceResult<IEnumerable<SportsAreaReadDto>>> GetAllAsync()
    {
        var areas = await _sportsAreaRepository.GetAllAsync();
        var dtos = areas.Select(a => new SportsAreaReadDto
        {
            Id = a.Id,
            Name = a.Name,
            SportType = a.SportType,
            MaxCapacity = a.MaxCapacity,
            HourlyRate = a.HourlyRate,
            IsActive = a.IsActive
        });
        return ServiceResult<IEnumerable<SportsAreaReadDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<SportsAreaReadDto>> GetByIdAsync(int id)
    {
        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null)
            return ServiceResult<SportsAreaReadDto>.Fail("Sports area not found.");

        return ServiceResult<SportsAreaReadDto>.Ok(new SportsAreaReadDto
        {
            Id = area.Id,
            Name = area.Name,
            SportType = area.SportType,
            MaxCapacity = area.MaxCapacity,
            HourlyRate = area.HourlyRate,
            IsActive = area.IsActive
        });
    }

    public async Task<ServiceResult<SportsAreaReadDto>> CreateAsync(SportsAreaCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return ServiceResult<SportsAreaReadDto>.Fail("Name is required.");
        if (string.IsNullOrWhiteSpace(dto.SportType))
            return ServiceResult<SportsAreaReadDto>.Fail("Sport type is required.");
        if (dto.MaxCapacity <= 0)
            return ServiceResult<SportsAreaReadDto>.Fail("Max capacity must be greater than zero.");
        if (dto.HourlyRate <= 0)
            return ServiceResult<SportsAreaReadDto>.Fail("Hourly rate must be greater than zero.");

        var area = new SportsArea
        {
            Name = dto.Name.Trim(),
            SportType = dto.SportType.Trim(),
            MaxCapacity = dto.MaxCapacity,
            HourlyRate = dto.HourlyRate
        };

        var created = await _sportsAreaRepository.AddAsync(area);

        return ServiceResult<SportsAreaReadDto>.Ok(new SportsAreaReadDto
        {
            Id = created.Id,
            Name = created.Name,
            SportType = created.SportType,
            MaxCapacity = created.MaxCapacity,
            HourlyRate = created.HourlyRate,
            IsActive = created.IsActive
        }, "Sports area created.");
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, SportsAreaCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.SportType))
            return ServiceResult<bool>.Fail("Name and sport type are required.");
        if (dto.MaxCapacity <= 0 || dto.HourlyRate <= 0)
            return ServiceResult<bool>.Fail("Capacity and hourly rate must be positive.");

        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null)
            return ServiceResult<bool>.Fail("Sports area not found.");

        area.Name = dto.Name.Trim();
        area.SportType = dto.SportType.Trim();
        area.MaxCapacity = dto.MaxCapacity;
        area.HourlyRate = dto.HourlyRate;

        await _sportsAreaRepository.UpdateAsync(area);
        return ServiceResult<bool>.Ok(true, "Sports area updated.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var area = await _sportsAreaRepository.GetByIdAsync(id);
        if (area == null)
            return ServiceResult<bool>.Fail("Sports area not found.");

        await _sportsAreaRepository.DeleteAsync(area);
        return ServiceResult<bool>.Ok(true, "Sports area deleted.");
    }
}