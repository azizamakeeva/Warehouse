using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.ViewModel;
namespace WebApplication2.Controllers
{
    public class RawMaterialPurchasesController : Controller
    {
        private readonly WarehouseContext _context;

        public RawMaterialPurchasesController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: RawMaterialPurchases
        public async Task<IActionResult> Index()
        {
            var warehouseContext = _context.RawMaterialPurchases.Include(r => r.EmployeeNavigation).Include(r => r.RawMaterialNavigation);
            return View(await warehouseContext.ToListAsync());
        }

        // GET: RawMaterialPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rawMaterialPurchase = await _context.RawMaterialPurchases
                .Include(r => r.EmployeeNavigation)
                .Include(r => r.RawMaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rawMaterialPurchase == null)
            {
                return NotFound();
            }

            return View(rawMaterialPurchase);
        }

        // GET: RawMaterialPurchases/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["RawMaterial"] = new SelectList(_context.RawMaterials, "Id", "Name");
            return View();
        }

        // POST: RawMaterialPurchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RawMaterial,Count,Amount,Date,Employee")] RawMaterialPurchase rawMaterialPurchase)
        {
            var budget = _context.Budgets.Where(u => u.Id == 1).FirstOrDefault();
            var raw = _context.RawMaterials.Where(u => u.Id == rawMaterialPurchase.RawMaterial).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (rawMaterialPurchase.Amount > budget.BudgetAmount)
                {
                    return NotFound("Недостаточно средств!");
                }
                else
                {
                    //budget.BudgetAmount -= (int)rawMaterialPurchase.Amount;
                    raw.Count += rawMaterialPurchase.Count;
                    raw.Sum += rawMaterialPurchase.Amount;
                    _context.Add(rawMaterialPurchase);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }

            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["RawMaterial"] = new SelectList(_context.RawMaterials, "Id", "Name");
            return View(rawMaterialPurchase);
        }

    // GET: RawMaterialPurchases/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rawMaterialPurchase = await _context.RawMaterialPurchases.FindAsync(id);
        if (rawMaterialPurchase == null)
        {
            return NotFound();
        }
        ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", rawMaterialPurchase.Employee);
        ViewData["RawMaterial"] = new SelectList(_context.RawMaterials, "Id", "Name", rawMaterialPurchase.RawMaterial);
        return View(rawMaterialPurchase);
    }

    // POST: RawMaterialPurchases/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,RawMaterial,Count,Amount,Date,Employee")] RawMaterialPurchase rawMaterialPurchase)
    {
        if (id != rawMaterialPurchase.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(rawMaterialPurchase);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RawMaterialPurchaseExists(rawMaterialPurchase.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", rawMaterialPurchase.Employee);
        ViewData["RawMaterial"] = new SelectList(_context.RawMaterials, "Id", "Name", rawMaterialPurchase.RawMaterial);
        return View(rawMaterialPurchase);
    }

    // GET: RawMaterialPurchases/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rawMaterialPurchase = await _context.RawMaterialPurchases
            .Include(r => r.EmployeeNavigation)
            .Include(r => r.RawMaterialNavigation)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (rawMaterialPurchase == null)
        {
            return NotFound();
        }

        return View(rawMaterialPurchase);
    }

    // POST: RawMaterialPurchases/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rawMaterialPurchase = await _context.RawMaterialPurchases.FindAsync(id);
        _context.RawMaterialPurchases.Remove(rawMaterialPurchase);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RawMaterialPurchaseExists(int id)
    {
        return _context.RawMaterialPurchases.Any(e => e.Id == id);
    }
  }

}