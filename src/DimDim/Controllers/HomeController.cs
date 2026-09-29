using Microsoft.AspNetCore.Mvc;
namespace DimDim.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
