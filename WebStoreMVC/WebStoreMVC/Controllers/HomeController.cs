using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebStoreMVC.Data;
using WebStoreMVC.Mapper;
using WebStoreMVC.Models;
using WebStoreMVC.Models.Seeder;

namespace WebStoreMVC.Controllers
{
    public class HomeController(MyContextShopMVC myContext,
        CategoryMapper categoryMapper,
        ProductMapper productMapper) : Controller
    {
        public IActionResult Index()
        {
            var items = myContext.Categories.ToList();
            var modal = categoryMapper.CategoriesToCategoryItems(items);
            return View(modal);
        }

        [HttpGet]
        public IActionResult Products(string categorySlug)
        {
            var items = myContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Where(x => x.Category.Slug == categorySlug)
                .ToList();
            var modal = productMapper.ListProductEntityToItemModels(items);
            return View(modal);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
