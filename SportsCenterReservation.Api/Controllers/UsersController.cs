namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Api.DTOs;
using SportsCenterReservation.Domain.Entities;
using SportsCenterReservation.Infrastructure.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        var result = users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            IdentificationNumber = u.IdentificationNumber,
            RegistrationDate = u.RegistrationDate,
            IsActive = u.IsActive
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(new UserResponseDto
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

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> RegisterUser(UserRegisterDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            return Conflict("A user with this email already exists.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IdentificationNumber = dto.IdentificationNumber
        };

        var created = await _userRepository.AddAsync(user);

        var response = new UserResponseDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            PhoneNumber = created.PhoneNumber,
            IdentificationNumber = created.IdentificationNumber,
            RegistrationDate = created.RegistrationDate,
            IsActive = created.IsActive
        };

        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserRegisterDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.IdentificationNumber = dto.IdentificationNumber;

        await _userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        await _userRepository.DeleteAsync(user);
        return NoContent();
    }
}