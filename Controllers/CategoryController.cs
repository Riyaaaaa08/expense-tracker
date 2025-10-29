using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models;

namespace ExpenseTracker.Web.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Where(c => c.UserId == UserId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(categories);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Category category)
    {
        category.UserId = UserId;

        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories
            .Where(c => c.Id == id && c.UserId == UserId)
            .FirstOrDefaultAsync();

        if (category == null) return NotFound();

        return View(category);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
    {
        if (id != category.Id) return NotFound();

        category.UserId = UserId;

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Category updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id && e.UserId == UserId))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories
            .Where(c => c.Id == id && c.UserId == UserId)
            .FirstOrDefaultAsync();

        if (category == null) return NotFound();

        // Check if category is in use
        var hasTransactions = await _context.Transactions
            .AnyAsync(t => t.CategoryId == id && t.UserId == UserId);

        ViewBag.HasTransactions = hasTransactions;

        return View(category);
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id && c.UserId == UserId)
            .FirstOrDefaultAsync();

        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}

