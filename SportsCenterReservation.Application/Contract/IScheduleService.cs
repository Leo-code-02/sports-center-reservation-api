using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Contract;

using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;

public interface IScheduleService
{
    Task<ServiceResult<IEnumerable<ScheduleReadDto>>> GetAllAsync();
    Task<ServiceResult<ScheduleReadDto>> GetByIdAsync(int id);
    Task<ServiceResult<ScheduleReadDto>> CreateAsync(ScheduleCreateDto dto);
    Task<ServiceResult<bool>> UpdateAsync(int id, ScheduleCreateDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}