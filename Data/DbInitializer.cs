using ExpenseTracker.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Web.Data;

public static class DbInitializer
{
    private static readonly string[] DefaultIncomeCategories = { "Salary", "Freelance", "Investment", "Other Income" };
    private static readonly string[] DefaultExpenseCategories = { "Food", "Travel", "Shopping", "Bills", "Entertainment", "Health", "Other Expense" };

    public static async Task SeedDefaultCategoriesForUser(UserManager<ApplicationUser> userManager, ApplicationDbContext context, string userId)
    {
        var existingCategories = context.Categories.Where(c => c.UserId == userId).Select(c => c.Name).ToHashSet();

        // Add income categories
        foreach (var category in DefaultIncomeCategories)
        {
            if (!existingCategories.Contains(category))
            {
                context.Categories.Add(new Category { UserId = userId, Name = category });
            }
        }

        // Add expense categories
        foreach (var category in DefaultExpenseCategories)
        {
            if (!existingCategories.Contains(category))
            {
                context.Categories.Add(new Category { UserId = userId, Name = category });
            }
        }

        await context.SaveChangesAsync();
    }
}

