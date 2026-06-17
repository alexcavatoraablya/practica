using System.ComponentModel.DataAnnotations;

namespace WebStoreMVC.Areas.Admin.Models.Product;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Обов'язкове поле")]
    [DataType(DataType.Text)]
    [Display(Name = "Назва продукту")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Ціна")]
    public string Price { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Категорія")]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Display(Name = "Зображення")]
    public List<CreateProductImageViewModel> Images { get; set; } = new();
}