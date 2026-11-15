using System;
using System.Collections.Generic;
using System.Text;

namespace SportsCenterReservation.Application.Service;

using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Core;
using SportsCenterReservation.Application.Dtos;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<IEnumerable<UserResponseDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var dtos = users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            IdentificationNumber = u.IdentificationNumber,
            RegistrationDate = u.RegistrationDate,
            IsActive = u.IsActive
        });
        return ServiceResult<IEnumerable<UserResponseDto>>.Ok(dtos);
    }

    public async Task<ServiceResult<UserResponseDto>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return ServiceResult<UserResponseDto>.Fail("User not found.");

        return ServiceResult<UserResponseDto>.Ok(new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IdentificationNumber = user.IdentificationNumber,
            RegistrationDate = user.RegistrationDate,
            IsActive = user.IsActive
        });
    }

    public async Task<ServiceResult<UserResponseDto>> RegisterAsync(UserRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return ServiceResult<UserResponseDto>.Fail("Full name is required.");
        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
            return ServiceResult<UserResponseDto>.Fail("A valid email is required.");
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            return ServiceResult<UserResponseDto>.Fail("Phone number is required.");
        if (string.IsNullOrWhiteSpace(dto.IdentificationNumber))
            return ServiceResult<UserResponseDto>.Fail("Identification number is required.");

        var existing = await _userRepository.GetByEmailAsync(dto.Email.Trim());
        if (existing != null)
            return ServiceResult<UserResponseDto>.Fail("A user with this email already exists.");

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            IdentificationNumber = dto.IdentificationNumber.Trim()
        };

        var created = await _userRepository.AddAsync(user);

        return ServiceResult<UserResponseDto>.Ok(new UserResponseDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            PhoneNumber = created.PhoneNumber,
            IdentificationNumber = created.IdentificationNumber,
            RegistrationDate = created.RegistrationDate,
            IsActive = created.IsActive
        }, "User registered.");
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, UserRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
            return ServiceResult<bool>.Fail("Valid full name and email are required.");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return ServiceResult<bool>.Fail("User not found.");

        user.FullName = dto.FullName.Trim();
        user.Email = dto.Email.Trim();
        user.PhoneNumber = dto.PhoneNumber.Trim();
        user.IdentificationNumber = dto.IdentificationNumber.Trim();

        await _userRepository.UpdateAsync(user);
        return ServiceResult<bool>.Ok(true, "User updated.");
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return ServiceResult<bool>.Fail("User not found.");

        await _userRepository.DeleteAsync(user);
        return ServiceResult<bool>.Ok(true, "User deleted.");
    }
}