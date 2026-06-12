using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStoreMVC.Data;
using WebStoreMVC.Mapper;

namespace WebStoreMVC.Controllers
{
    public class ProductsController(MyContextShopMVC myContext,
        ProductMapper productMapper) : Controller
    {
        public IActionResult Details(string slug)
        {
            Console.WriteLine("Slug = "+slug);
            //читвє товари з БД і відображає його
            var item = myContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .SingleOrDefault(x => x.Slug == slug);
            var model = productMapper
                .ProductEntityToProductItemModel(item);
            return View(model);
        }
    }
}
