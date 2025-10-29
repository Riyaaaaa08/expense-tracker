using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Web.Data;
using ExpenseTracker.Web.Models;

namespace ExpenseTracker.Web.Controllers;

[Authorize]
public class TransactionController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransactionController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: Transaction
    public async Task<IActionResult> Index(TransactionType? type, int? categoryId, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == UserId);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= DateOnly.FromDateTime(startDate.Value));

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= DateOnly.FromDateTime(endDate.Value));

        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();

        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.UserId == UserId).ToListAsync(), "Id", "Name");
        return View(transactions);
    }

    // GET: Transaction/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.UserId == UserId).ToListAsync(), "Id", "Name");
        return View();
    }

    // POST: Transaction/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Date,Amount,Description,Type,CategoryId")] Transaction transaction)
    {
        transaction.UserId = UserId;

        if (ModelState.IsValid)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Transaction created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.UserId == UserId).ToListAsync(), "Id", "Name");
        return View(transaction);
    }

    // GET: Transaction/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _context.Transactions
            .Where(t => t.Id == id && t.UserId == UserId)
            .FirstOrDefaultAsync();

        if (transaction == null) return NotFound();

        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.UserId == UserId).ToListAsync(), "Id", "Name");
        return View(transaction);
    }

    // POST: Transaction/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Amount,Description,Type,CategoryId")] Transaction transaction)
    {
        if (id != transaction.Id) return NotFound();

        transaction.UserId = UserId;

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Transaction updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transactions.Any(e => e.Id == id && e.UserId == UserId))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => c.UserId == UserId).ToListAsync(), "Id", "Name");
        return View(transaction);
    }

    // GET: Transaction/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.Id == id && t.UserId == UserId)
            .FirstOrDefaultAsync();

        if (transaction == null) return NotFound();

        return View(transaction);
    }

    // POST: Transaction/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transaction = await _context.Transactions
            .Where(t => t.Id == id && t.UserId == UserId)
            .FirstOrDefaultAsync();

        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Transaction deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}

