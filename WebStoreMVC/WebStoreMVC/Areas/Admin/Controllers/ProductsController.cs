using Microsoft.AspNetCore.Mvc;
using WebStoreMVC.Areas.Admin.Models.Product;
using WebStoreMVC.Data;

namespace WebStoreMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController(
    MyContextShopMVC myContext
    ) : Controller
{
    public IActionResult Index()
    {
        ViewBag.Title = "Продукти";
        var model = myContext.Products.Select(x =>
        new ProductItemViewModel
        {
            CategoryName = x.Category.Name,
            Images = x.ProductImages
            .OrderBy(x => x.Priority)
            .Select(x=>x.Name)
            .ToList(),
            Name = x.Name,
            Description = x.Description,
        }).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Craete()
    {
        ViewBag.Title = "Створити продукт";
        ViewBag.Categories = myContext.Categories.ToList();
        return View();
    }
}
