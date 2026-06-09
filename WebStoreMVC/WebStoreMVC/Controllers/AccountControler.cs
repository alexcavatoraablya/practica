using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStoreMVC.Constants;
using WebStoreMVC.Data.Entities.Identity;
using WebStoreMVC.Interfaces;
using WebStoreMVC.Models.Account;

namespace WebStoreMVC.Controllers;

public class AccountController(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IImageService imageService
    ) : Controller
{
    [HttpGet] //Реєстрація нового користувача
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost] //Реєстрація нового користувача
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid) //Зберігаємо категорію в БД, якщо модель валідна
        {
            string fileName = "default.jpg";
            //Як зберегти фото
            if (model.FileImage != null)
            {
                fileName = await imageService.SaveImageAsync(model.FileImage);
            }
            //Заповнюю таблицю коритувачів в БД
            var user = new UserEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                Image = fileName
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(user, Roles.User);
                await signInManager.SignInAsync(user, false); //логін користувача
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Помилка при реєстрації користувача");
                return View(model);
            }
        }
        return View(model); // Якщо модель не валідна, повертаємо її назад на форму для виправлення помилок
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }
}

