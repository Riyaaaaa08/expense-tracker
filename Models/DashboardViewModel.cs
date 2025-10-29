namespace ExpenseTracker.Web.Models;

public class DashboardViewModel
{
	public decimal TotalIncome { get; set; }
	public decimal TotalExpense { get; set; }
	public decimal Balance => TotalIncome - TotalExpense;

	public IReadOnlyList<(string Category, decimal Amount)> TopCategories { get; set; } = Array.Empty<(string, decimal)>();
	public IReadOnlyList<(int Month, decimal Income, decimal Expense)> MonthlySummary { get; set; } = Array.Empty<(int, decimal, decimal)>();
}


