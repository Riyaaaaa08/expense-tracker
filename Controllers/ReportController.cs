using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models;

namespace ExpenseTracker.Web.Controllers;

[Authorize]
public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: Report
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1);
        
        // Monthly summary for last 6 months
        var monthlyData = new List<(int Month, decimal Income, decimal Expense)>();
        
        for (int i = 5; i >= 0; i--)
        {
            var monthStart = currentMonthStart.AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            
            var incomeList = await _context.Transactions
                .Where(t => t.UserId == UserId && 
                           t.Type == TransactionType.Income &&
                           t.Date >= DateOnly.FromDateTime(monthStart) &&
                           t.Date <= DateOnly.FromDateTime(monthEnd))
                .ToListAsync();
            var income = incomeList.Sum(t => t.Amount);
            
            var expenseList = await _context.Transactions
                .Where(t => t.UserId == UserId && 
                           t.Type == TransactionType.Expense &&
                           t.Date >= DateOnly.FromDateTime(monthStart) &&
                           t.Date <= DateOnly.FromDateTime(monthEnd))
                .ToListAsync();
            
            var expense = expenseList.Sum(t => t.Amount);
            
            monthlyData.Add((monthStart.Month, income, expense));
        }
        
        // Top spending categories (using ToListAsync for SQLite compatibility)
        var expenseTransactions = await _context.Transactions
            .Where(t => t.UserId == UserId && t.Type == TransactionType.Expense)
            .Include(t => t.Category)
            .ToListAsync();
        
        var topCategories = expenseTransactions
            .GroupBy(t => t.Category?.Name ?? "Unknown")
            .Select(g => new { Category = g.Key, Amount = g.Sum(t => t.Amount) })
            .OrderByDescending(g => g.Amount)
            .Take(5)
            .ToList();
        
        var topCatData = topCategories.Select(c => (c.Category, c.Amount)).ToList();
        
        ViewBag.MonthlyData = monthlyData;
        ViewBag.TopCategories = topCatData;
        
        return View();
    }
}

