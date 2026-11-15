namespace SportsCenterReservation.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SportsCenterReservation.Application.Contract;
using SportsCenterReservation.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class SportsAreasController : ControllerBase
{
    private readonly ISportsAreaService _sportsAreaService;

    public SportsAreasController(ISportsAreaService sportsAreaService)
    {
        _sportsAreaService = sportsAreaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSportsAreas()
    {
        var result = await _sportsAreaService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSportsArea(int id)
    {
        var result = await _sportsAreaService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSportsArea(SportsAreaCreateDto dto)
    {
        var result = await _sportsAreaService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);

        return CreatedAtAction(nameof(GetSportsArea), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSportsArea(int id, SportsAreaCreateDto dto)
    {
        var result = await _sportsAreaService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSportsArea(int id)
    {
        var result = await _sportsAreaService.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}