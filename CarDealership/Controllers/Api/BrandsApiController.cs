using CarDealership.DTOs;
using CarDealership.Mappings;
using CarDealership.Models;
using CarDealership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers.Api;

[Route("api/brands")]
[ApiController]
public class BrandsApiController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsApiController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET: /api/brands
    [HttpGet]
    [ProducesResponseType(typeof(List<BrandDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BrandDto>>> GetAll(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllBrandsAsync(cancellationToken);
        return Ok(brands.ToDtoList());
    }

    // GET: /api/brands/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrandDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
        if (brand == null)
            return NotFound();

        return Ok(brand.ToDto());
    }

    // POST: /api/brands
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var brand = new Brand { Name = dto.Name };
        await _brandService.CreateBrandAsync(brand, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand.ToDto());
    }

    // PUT: /api/brands/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
        if (brand == null)
            return NotFound();

        brand.Name = dto.Name;
        await _brandService.UpdateBrandAsync(brand, cancellationToken);

        return NoContent();
    }

    // DELETE: /api/brands/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetBrandByIdAsync(id, cancellationToken);
        if (brand == null)
            return NotFound();

        await _brandService.DeleteBrandAsync(id, cancellationToken);
        return NoContent();
    }
}
