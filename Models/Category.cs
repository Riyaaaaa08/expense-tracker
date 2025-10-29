using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Web.Models;

public class Category
{
	public int Id { get; set; }

	[Required, MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	[Required]
	public string UserId { get; set; } = string.Empty; // FK to AspNetUsers
}


