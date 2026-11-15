using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Contract;

using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;

public interface ISportsAreaService
{
    Task<ServiceResult<IEnumerable<SportsAreaReadDto>>> GetAllAsync();
    Task<ServiceResult<SportsAreaReadDto>> GetByIdAsync(int id);
    Task<ServiceResult<SportsAreaReadDto>> CreateAsync(SportsAreaCreateDto dto);
    Task<ServiceResult<bool>> UpdateAsync(int id, SportsAreaCreateDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}