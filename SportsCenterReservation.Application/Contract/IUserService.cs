using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Contract;

using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;

public interface IUserService
{
    Task<ServiceResult<IEnumerable<UserResponseDto>>> GetAllAsync();
    Task<ServiceResult<UserResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<UserResponseDto>> RegisterAsync(UserRegisterDto dto);
    Task<ServiceResult<bool>> UpdateAsync(int id, UserRegisterDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}