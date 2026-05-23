using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers;

[Authorize(Roles = "Admin")]
public class FeaturesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}