using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}