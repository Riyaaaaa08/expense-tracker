using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models;

namespace ExpenseTracker.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return RedirectToAction("Login", "Identity");

        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        // Get current month totals (using ToListAsync for SQLite decimal compatibility)
        var incomeTransactions = await _context.Transactions
            .Where(t => t.UserId == userId && 
                       t.Type == TransactionType.Income &&
                       t.Date >= DateOnly.FromDateTime(monthStart) &&
                       t.Date <= DateOnly.FromDateTime(monthEnd))
            .ToListAsync();
        
        var totalIncome = incomeTransactions.Sum(t => t.Amount);

        var expenseTransactions = await _context.Transactions
            .Where(t => t.UserId == userId &&
                       t.Type == TransactionType.Expense &&
                       t.Date >= DateOnly.FromDateTime(monthStart) &&
                       t.Date <= DateOnly.FromDateTime(monthEnd))
            .ToListAsync();
        
        var totalExpense = expenseTransactions.Sum(t => t.Amount);

        // Get monthly data for last 6 months
        var monthlyData = new List<(int Month, decimal Income, decimal Expense)>();
        
        for (int i = 5; i >= 0; i--)
        {
            var ms = monthStart.AddMonths(-i);
            var me = ms.AddMonths(1).AddDays(-1);
            
            var incList = await _context.Transactions
                .Where(t => t.UserId == userId && 
                           t.Type == TransactionType.Income &&
                           t.Date >= DateOnly.FromDateTime(ms) &&
                           t.Date <= DateOnly.FromDateTime(me))
                .ToListAsync();
            
            var inc = incList.Sum(t => t.Amount);
            
            var expList = await _context.Transactions
                .Where(t => t.UserId == userId && 
                           t.Type == TransactionType.Expense &&
                           t.Date >= DateOnly.FromDateTime(ms) &&
                           t.Date <= DateOnly.FromDateTime(me))
                .ToListAsync();
            
            var exp = expList.Sum(t => t.Amount);
            
            monthlyData.Add((ms.Month, inc, exp));
        }

        // Top spending categories (using ToListAsync for SQLite compatibility)
        var allExpenseTransactions = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .Include(t => t.Category)
            .ToListAsync();
        
        var topCategories = allExpenseTransactions
            .GroupBy(t => t.Category?.Name ?? "Unknown")
            .Select(g => new { Category = g.Key, Amount = g.Sum(t => t.Amount) })
            .OrderByDescending(g => g.Amount)
            .Take(5)
            .ToList();

        var viewModel = new DashboardViewModel
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            MonthlySummary = monthlyData,
            TopCategories = topCategories.Select(c => (c.Category, c.Amount)).ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
