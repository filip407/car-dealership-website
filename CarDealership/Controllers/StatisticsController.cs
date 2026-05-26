using CarDealership.Data;
using CarDealership.ViewModels;
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
            .ThenInclude(c => c!.Brand)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        var viewModel = new StatisticsViewModel
        {
            TotalRevenue = sales.Sum(s => s.SalePrice),
            TotalCarsSold = sales.Count,
            AgentStats = sales
                .GroupBy(s => s.Agent?.FullName ?? "Agent Necunoscut")
                .Select(g => new AgentStatViewModel
                {
                    AgentName = g.Key,
                    CarsSold = g.Count(),
                    Revenue = g.Sum(s => s.SalePrice)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList(),
            RecentSales = sales.Take(10).Select(s => new RecentSaleViewModel
            {
                CarName = $"{s.Car?.Brand?.Name} {s.Car?.ModelName}".Trim(),
                AgentName = s.Agent?.FullName ?? string.Empty,
                SaleDate = s.SaleDate,
                SalePrice = s.SalePrice
            }).ToList()
        };

        return View(viewModel);
    }
}