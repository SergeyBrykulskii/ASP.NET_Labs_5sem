using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.Models;

namespace Web_153501_Brykulskii.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var listDemo = new List<ListDemo>
        {
            new ListDemo { Id =  1, Name = "Item 1"},
            new ListDemo { Id = 2, Name = "Item 2"},
            new ListDemo { Id = 3, Name = "Item 3"}
        };

        ViewData["MainHeaderMessage"] = "Лабораторная работа №2";
        ViewData["ItemsDemoList"] = listDemo;

        return View();
    }
}
