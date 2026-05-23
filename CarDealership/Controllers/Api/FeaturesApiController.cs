using CarDealership.Data;
using CarDealership.DTOs;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers.Api;

[Route("api/features")]
[ApiController]
public class FeaturesApiController : ControllerBase
{
	private readonly AppDbContext _context;

	public FeaturesApiController(AppDbContext context)
	{
		_context = context;
	}

	// 1. GET /api/features -> 200 OK
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var features = await _context.Features
			.Select(f => new FeatureDto { Id = f.Id, Name = f.Name })
			.ToListAsync();

		return Ok(features); // Status 200
	}

	// 2. GET /api/features/{id} -> 200 OK / 404 Not Found
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var feature = await _context.Features.FindAsync(id);

		if (feature == null)
			return NotFound(); // Status 404

		var dto = new FeatureDto { Id = feature.Id, Name = feature.Name };
		return Ok(dto); // Status 200
	}

	// 3. POST /api/features -> 201 Created / 400 Bad Request
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] FeatureDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState); // Status 400

		var feature = new Feature { Name = dto.Name };
		_context.Features.Add(feature);
		await _context.SaveChangesAsync();

		dto.Id = feature.Id;

		// Status 201 cu Location Header
		return CreatedAtAction(nameof(GetById), new { id = feature.Id }, dto);
	}

	// 4. PUT /api/features/{id} -> 204 No Content / 400 / 404
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] FeatureDto dto)
	{
		if (id != dto.Id || !ModelState.IsValid)
			return BadRequest(); // Status 400

		var feature = await _context.Features.FindAsync(id);
		if (feature == null)
			return NotFound(); // Status 404

		feature.Name = dto.Name;
		await _context.SaveChangesAsync();

		return NoContent(); // Status 204
	}

	// 5. DELETE /api/features/{id} -> 204 No Content / 404 Not Found
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var feature = await _context.Features.FindAsync(id);
		if (feature == null)
			return NotFound(); // Status 404

		_context.Features.Remove(feature);
		await _context.SaveChangesAsync();

		return NoContent(); // Status 204
	}
}