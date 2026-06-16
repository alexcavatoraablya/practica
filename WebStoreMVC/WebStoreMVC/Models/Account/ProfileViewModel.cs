namespace WebStoreMVC.Models.Account;

public class ProfileViewModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? Image { get; set; }

    public int OrdersCount { get; set; }
}