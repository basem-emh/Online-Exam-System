using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Services.Admin;
using OnlineExamSystem.PL.Models;
using System.Diagnostics;

namespace OnlineExamSystem.PL.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IAdminServices _adminServices;

    public HomeController(IAdminServices adminServices)
    {
        _adminServices = adminServices;
    }

    public async Task<IActionResult> Index()
    {
        var users =await _adminServices.GetAllExamsAsync();
        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
