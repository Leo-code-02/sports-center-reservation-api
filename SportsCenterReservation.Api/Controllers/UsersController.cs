namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(UserRegisterDto dto)
    {
        var result = await _userService.RegisterAsync(dto);
        if (!result.Success) return BadRequest(result);

        return CreatedAtAction(nameof(GetUser), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserRegisterDto dto)
    {
        var result = await _userService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}