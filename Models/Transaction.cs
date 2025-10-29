using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models;

public enum TransactionType
{
	Income = 1,
	Expense = 2
}

public class Transaction
{
	public int Id { get; set; }

	[DataType(DataType.Date)]
	public DateOnly Date { get; set; }

	[Range(0.01, double.MaxValue)]
	public decimal Amount { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	[Required]
	public TransactionType Type { get; set; }

	// Relationships
	public int CategoryId { get; set; }
	public Category? Category { get; set; }

	[Required]
	public string UserId { get; set; } = string.Empty; // FK to AspNetUsers
}


