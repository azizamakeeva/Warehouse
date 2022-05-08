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
    public class ProductionsController : Controller
    {
        private readonly WarehouseContext _context;

        public ProductionsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Productions
        public async Task<IActionResult> Index()
        {
            var warehouseContext = _context.Productions.Include(p => p.EmployeeNavigation).Include(p => p.ProductNavigation);
            return View(await warehouseContext.ToListAsync());
        }

        // GET: Productions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Productions
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (production == null)
            {
                return NotFound();
            }

            return View(production);
        }

        // GET: Productions/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }

        // POST: Productions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Count,Date,Employee")] Production production)
        {
            if (ModelState.IsValid)
            {
                WarehouseContext db = new WarehouseContext();
                List<Ingridient> ingredients = new List<Ingridient>();
                List<RawMaterial> rawMaterials = new List<RawMaterial>();

                ingredients = ((from count in _context.Ingridients
                                where count.Products == production.Product
                                select count).ToList());

                foreach (var item in ingredients)
                {
                    rawMaterials.Add((from ing in _context.RawMaterials
                                      where ing.Id == item.RawMaterials
                                      select ing).First());
                }

                bool isNotEnogh = false;
                foreach (var rawM in rawMaterials)
                {
                    foreach (var ingred in ingredients)
                    {
                        if (rawM.Id == ingred.RawMaterials)
                        {
                            if (rawM.Count < (ingred.Count * production.Count))
                            {
                                isNotEnogh = true;
                                break;
                            }
                        }
                    }
                }
                decimal averageSum, needSum, totalSum = 0;
                float needQuantity;
                if (isNotEnogh)
                {
                    ModelState.AddModelError("Quantity", "Not enough Raw Material! Choose less or buy more Raw Materials");
                }
                else if (!isNotEnogh)
                {
                    foreach (var rawM in rawMaterials)
                    {
                        foreach (var ingred in ingredients)
                        {
                            if (rawM.Id == ingred.RawMaterials)
                            {
                                averageSum = (decimal)(rawM.Sum / rawM.Count);
                                needQuantity = (float)(ingred.Count * production.Count);
                                needSum = averageSum * Convert.ToDecimal(needQuantity);

                                var productCount = db.RawMaterials
                                    .Where(r => r.Id == ingred.RawMaterials)
                                    .FirstOrDefault();
                                productCount.Count = (int?)(productCount.Count - needQuantity);
                                productCount.Sum = ((int?)(productCount.Sum - needSum));
                                db.SaveChanges();

                                totalSum += needSum;
                            }

                        }
                    }

                    var readyProducts = db.FinishedProducts
                           .Where(r => r.Id == production.Product)
                           .FirstOrDefault();
                    readyProducts.Count = (readyProducts.Count + production.Count);
                    readyProducts.Sum = readyProducts.Sum + totalSum;
                    db.SaveChanges();
                    _context.Add(production);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }


            //if (ModelState.IsValid)
            //{
            //    var сountPR = production.Amount;
            //    var ID = production.Product;

            //    StorageContext db = new StorageContext();
            //    List<Materials> rawmaterials = new List<Materials>();
            //    List<Ingredient> ingredients2 = new List<Ingredient>();
            //    List<Products> finproducts = new List<Products>();

            //    rawmaterials = ((from value in _context.Materials
            //                     join c in _context.Ingredient on value.Id equals c.Material
            //                     where c.Product == ID
            //                     select value).ToList());
            //    ingredients2 = ((from value in _context.Ingredient
            //                     join c in _context.Ingredient on value.Id equals c.Id
            //                     where c.Product == ID
            //                     select value).ToList());

            //    int IdOfProduct = production.Product;
            //    int i = 0;
            //    bool zero = false;
            //    while (rawmaterials.Count > i)
            //    {
            //        var IdOfMaterial = rawmaterials[i].Id;
            //        var AmmOfMaterial = rawmaterials[i].Amount;
            //        int j = 0;
            //        while (ingredients2.Count > j)
            //        {
            //            var IdOfMaterialFromIngr = ingredients2[j].Material;
            //            var AmmOfNeededMaterial = ingredients2[j].Amount;
            //            var SummOfNeeded = AmmOfNeededMaterial * сountPR;
            //            if (AmmOfMaterial < SummOfNeeded)
            //            {
            //                zero = true;
            //            }
            //            j++;
            //        }
            //        i++;
            //    }
            //    if (zero)
            //    {
            //        ModelState.AddModelError("Amount", "Не хватает количества сырья!");
            //    }
            //    else
            //    {

            //        var SummaRaw = ((from value in _context.Materials
            //                         join c in _context.Ingredient on value.Id equals c.Material
            //                         where c.Product == ID
            //                         select (value.Cost / value.Amount * c.Amount * сountPR))).ToList();



            //        var Summa = ((from value in _context.Materials
            //                      join c in _context.Ingredient on value.Id equals c.Material
            //                      where c.Product == ID
            //                      select ((value.Cost / value.Amount) * c.Amount * сountPR))).Sum();


            //        var countIngr = ((from value in _context.Ingredient
            //                          join c in _context.Materials on value.Material equals c.Id
            //                          where value.Product == ID
            //                          select value.Amount * сountPR).ToList());



            //        finproducts = ((from col in _context.Products
            //                        where col.Id == ID
            //                        select col).ToList());

            //        decimal sumfinpr = (decimal)finproducts[0].Cost;
            //        decimal countfinpr = (decimal)finproducts[0].Amount;

            //        decimal summ = sumfinpr + (decimal)Summa;
            //        decimal colfin = countfinpr + (decimal)сountPR;
            //        var Updateprod = db.Products
            //            .Where(id => id.Id == ID)
            //            .First();
            //        Updateprod.Cost = (double)summ;
            //        Updateprod.Amount = (int)colfin;
            //        db.SaveChanges();


            //        i = 0;
            //        while (SummaRaw.Count > i)
            //        {
            //            decimal v = (decimal)SummaRaw[i];
            //            decimal col = (decimal)countIngr[i];
            //            int IDraw = rawmaterials[i].Id;
            //            var UpdateRawMaterial = db.Materials
            //           .Where(id => id.Id == IDraw)
            //           .First();
            //            UpdateRawMaterial.Cost = UpdateRawMaterial.Cost - (double)v;
            //            UpdateRawMaterial.Amount = UpdateRawMaterial.Amount - (double)col;
            //            db.SaveChanges();
            //            i++;
            //        }

            //        _context.Add(production);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //}


            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", production.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", production.Product);
            return View(production);
        }

        // GET: Productions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Productions.FindAsync(id);
            if (production == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", production.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", production.Product);
            return View(production);
        }

        // POST: Productions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Count,Date,Employee")] Production production)
        {
            if (id != production.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(production);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionExists(production.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", production.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name", production.Product);
            return View(production);
        }

        // GET: Productions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var production = await _context.Productions
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (production == null)
            {
                return NotFound();
            }

            return View(production);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var production = await _context.Productions.FindAsync(id);
            _context.Productions.Remove(production);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionExists(int id)
        {
            return _context.Productions.Any(e => e.Id == id);
        }
    }
}
