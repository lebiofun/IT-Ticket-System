using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Models;

public class ApplicationUser : IdentityUser
{
    public string Department { get; set; } = string.Empty;
}
