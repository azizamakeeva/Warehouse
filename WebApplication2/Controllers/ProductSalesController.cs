using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ProductSalesController : Controller
    {
        private readonly WarehouseContext _context;

        public ProductSalesController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: ProductSales
        public async Task<IActionResult> Index()
        {
            var warehouseContext = _context.ProductSales.Include(p => p.EmployeeNavigation).Include(p => p.ProductNavigation);
            return View(await warehouseContext.ToListAsync());
        }

        // GET: ProductSales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSales
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSale == null)
            {
                return NotFound();
            }

            return View(productSale);
        }

        // GET: ProductSales/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }

        // POST: ProductSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Count,Amount,Date,Employee")] ProductSale productSale)
        {
            if (ModelState.IsValid)
            {
                WarehouseContext db = new WarehouseContext();
                List<Budget> budget = new List<Budget>();
                List<FinishedProduct> readyProducts = new List<FinishedProduct>();

                budget = (from Budgetamount in _context.Budgets
                          select Budgetamount).ToList();

                readyProducts = ((from col in _context.FinishedProducts
                                  where col.Id == productSale.Product
                                  select col).ToList());

                FinishedProduct readyProduct1 = new FinishedProduct();
                Budget budgets = new Budget();

                decimal quantityReadyProducts = Convert.ToDecimal(readyProducts[0].Sum);
                decimal budgetAmount = Convert.ToDecimal(budget[0].BudgetAmount);
                decimal valsum, valcount, oneProductPercentage, sum_umn_kol;
                sum_umn_kol = Convert.ToDecimal(productSale.Amount) * Convert.ToDecimal(productSale.Count);

                if (Convert.ToDecimal(quantityReadyProducts) < Convert.ToDecimal(productSale.Count) && Convert.ToDecimal(budgetAmount) < Convert.ToDecimal(sum_umn_kol))
                {
                    ModelState.AddModelError("Amount", "Недостаточно продукта! Замените на меньшее количество!");
                }
                else
                {
                    //oneProduct = Convert.ToDecimal(productSales.Sum) / Convert.ToDecimal(productSales.Amount);
                    oneProductPercentage = Convert.ToDecimal(productSale.Amount * budget[0].BudgetPercent) / 100;
                    valsum = Convert.ToDecimal(budgetAmount) + Convert.ToDecimal(productSale.Amount) * Convert.ToDecimal(productSale.Count) + (Convert.ToDecimal(oneProductPercentage) * Convert.ToDecimal(productSale.Count));
                    valcount = Convert.ToDecimal(quantityReadyProducts) - Convert.ToDecimal(productSale.Count);

                    var col = db.Budgets
                        .Where(c => c.BudgetAmount == budgetAmount)
                        .FirstOrDefault();
                    col.BudgetAmount = (int)valsum;
                    db.SaveChanges();

                    var colRaw = db.FinishedProducts
                        .Where(r => r.Id == productSale.Product)
                        .FirstOrDefault();
                    colRaw.Count = (short?)valcount;
                    db.SaveChanges();

                    var sum_umn_kolRaw = db.FinishedProducts
                        .Where(r => r.Id == productSale.Product)
                        .FirstOrDefault();
                    colRaw.Count = (short?)valcount;
                    db.SaveChanges();

                    _context.Add(productSale);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", productSale.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", productSale.Product);
            return View(productSale);
        }

        // GET: ProductSales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSales.FindAsync(id);
            if (productSale == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", productSale.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", productSale.Product);
            return View(productSale);
        }

        // POST: ProductSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Count,Amount,Date,Employee")] ProductSale productSale)
        {
            if (id != productSale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSaleExists(productSale.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", productSale.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", productSale.Product);
            return View(productSale);
        }

        // GET: ProductSales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSales
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSale == null)
            {
                return NotFound();
            }

            return View(productSale);
        }

        // POST: ProductSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSale = await _context.ProductSales.FindAsync(id);
            _context.ProductSales.Remove(productSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSaleExists(int id)
        {
            return _context.ProductSales.Any(e => e.Id == id);
        }
    }
}
