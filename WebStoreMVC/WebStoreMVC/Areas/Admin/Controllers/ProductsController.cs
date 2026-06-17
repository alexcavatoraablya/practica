using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStoreMVC.Areas.Admin.Models.Product;
using WebStoreMVC.Constants;
using WebStoreMVC.Data;
using WebStoreMVC.Data.Entities.Catalog;
using WebStoreMVC.Interfaces;
using WebStoreMVC.Mapper;

namespace WebStoreMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{Roles.Admin}")]
public class ProductsController(
    MyContextShopMVC myContext,
    ProductMapper productMapper,
    IImageService imageService
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
                    .Select(x => x.Name)
                    .ToList(),
                Name = x.Name,
                Description = x.Description,
            }).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Title = "Створити продукт";
        ViewBag.Categories = myContext.Categories.ToList();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        var existingProduct = await myContext.Products.SingleOrDefaultAsync(x => x.Name == model.Name);

        if (existingProduct != null)
        {
            ModelState.AddModelError("Name", "Такий продукт вже є!!!");
            return View(model);
        }

        var productEntity = productMapper.CreateProductToProductEntity(model);

        productEntity.Category = await myContext.Categories
            .SingleOrDefaultAsync(x => x.Name == model.CategoryName);

        var savedImages = await Task.WhenAll(
            model.Images.Select(async image => new ProductImageEntity
            {
                Name = await imageService.SaveImageFromBase64Async(image.Name),
                Priority = image.Priority
            })
        );

        productEntity.ProductImages = savedImages.ToList();

        myContext.Products.Add(productEntity);
        await myContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}
