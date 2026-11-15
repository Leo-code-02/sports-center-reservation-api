using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Contract;

using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;

public interface IReservationService
{
    Task<ServiceResult<IEnumerable<ReservationResponseDto>>> GetAllAsync();
    Task<ServiceResult<ReservationResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<ReservationResponseDto>> CreateAsync(ReservationCreateDto dto);
    Task<ServiceResult<bool>> CancelAsync(int id);
}