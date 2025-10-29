using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Web.Models;

public class ApplicationUser : IdentityUser
{
	public string? FullName { get; set; }
	public string? ThemePreference { get; set; }
}


