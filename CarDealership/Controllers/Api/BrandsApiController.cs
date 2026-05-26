using CarDealership.DTOs;
using CarDealership.Mappings;
using CarDealership.Services;
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
}
