using CarDealership.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class StatisticsController : Controller
{
	private readonly AppDbContext _context;

	public StatisticsController(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		var sales = await _context.Sales
			.Include(s => s.Agent)
			.Include(s => s.Car)
			.ThenInclude(c => c.Brand)
			.OrderByDescending(s => s.SaleDate)
			.ToListAsync();

		return View(sales);
	}
}