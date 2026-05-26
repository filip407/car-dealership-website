using CarDealership.DTOs;
using CarDealership.Mappings;
using CarDealership.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers.Api;

[Route("api/cars")]
[ApiController]
public class CarsApiController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsApiController(ICarService carService)
    {
        _carService = carService;
    }

    // GET: /api/cars
    [HttpGet]
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarDto>>> GetAll(CancellationToken cancellationToken)
    {
        var cars = await _carService.GetAllCarsAsync(cancellationToken);
        return Ok(cars.ToDtoList());
    }

    // GET: /api/cars/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var car = await _carService.GetCarByIdAsync(id, cancellationToken);
        if (car == null)
            return NotFound();

        return Ok(car.ToDto());
    }
}
