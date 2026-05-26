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

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var features = await _context.Features
			.Select(f => new FeatureDto { Id = f.Id, Name = f.Name })
			.ToListAsync();

		return Ok(features);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var feature = await _context.Features.FindAsync(id);

		if (feature == null)
			return NotFound();

		var dto = new FeatureDto { Id = feature.Id, Name = feature.Name };
		return Ok(dto);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] FeatureDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var feature = new Feature { Name = dto.Name };
		_context.Features.Add(feature);
		await _context.SaveChangesAsync();

		dto.Id = feature.Id;

		return CreatedAtAction(nameof(GetById), new { id = feature.Id }, dto);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] FeatureDto dto)
	{
		if (id != dto.Id || !ModelState.IsValid)
			return BadRequest();

		var feature = await _context.Features.FindAsync(id);
		if (feature == null)
			return NotFound();

		feature.Name = dto.Name;
		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var feature = await _context.Features.FindAsync(id);
		if (feature == null)
			return NotFound();

		_context.Features.Remove(feature);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
