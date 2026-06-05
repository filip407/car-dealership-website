using CarDealership.DTOs;
using CarDealership.Mappings;
using CarDealership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers.Api;

[Route("api/features")]
[ApiController]
public class FeaturesApiController : ControllerBase
{
    private readonly IFeatureService _featureService;

    public FeaturesApiController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FeatureDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FeatureDto>>> GetAll(CancellationToken cancellationToken)
    {
        var features = await _featureService.GetAllFeaturesAsync(cancellationToken);
        return Ok(features.ToDtoList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FeatureDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FeatureDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var feature = await _featureService.GetFeatureByIdAsync(id, cancellationToken);
        if (feature == null)
            return NotFound();

        return Ok(feature.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(FeatureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<FeatureDto>> Create([FromBody] CreateFeatureDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var feature = dto.ToEntity();
        await _featureService.CreateFeatureAsync(feature, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = feature.Id }, feature.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFeatureDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var feature = await _featureService.GetFeatureByIdAsync(id, cancellationToken);
        if (feature == null)
            return NotFound();

        dto.ApplyTo(feature);
        await _featureService.UpdateFeatureAsync(feature, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var feature = await _featureService.GetFeatureByIdAsync(id, cancellationToken);
        if (feature == null)
            return NotFound();

        await _featureService.DeleteFeatureAsync(id, cancellationToken);
        return NoContent();
    }
}
