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
    public class IngridientsController : Controller
    {
        private readonly WarehouseContext _context;

        public IngridientsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: Ingridients
        public async Task<IActionResult> Index(string search_item)
        {
            ViewBag.Prod = new SelectList(_context.FinishedProducts, "Id", "Name");
            var warehouseContext = _context.Ingridients.Include(i => i.ProductsNavigation).Include(i => i.RawMaterialsNavigation);
            var ingred = from s in _context.Ingridients.Include(i => i.ProductsNavigation).Include(i => i.RawMaterialsNavigation) select s;
            if (!String.IsNullOrEmpty(search_item) && search_item != "все")
            {
                ingred = ingred.Where(s => s.ProductsNavigation.Name.Contains(search_item));
                return View(ingred.ToList());
            }
            return View(await warehouseContext.ToListAsync());



            //var warehouseContext = _context.Ingridients.Include(i => i.ProductsNavigation).Include(i => i.RawMaterialsNavigation);
            //var ingred = from s in _context.Ingridients.Include(i => i.ProductsNavigation).Include(i => i.RawMaterialsNavigation) select s;

            //if (!String.IsNullOrEmpty(searchString) && searchString != "все")
            //{
            //    ingred = ingred.Where(s => s.ProductsNavigation.Name.Contains(search_item));

            //    return View(ingred.ToList());
            //}
            //else
            //{
            //    return View(await warehouseContext.ToListAsync());
            //}



        }

        // GET: Ingridients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingridient = await _context.Ingridients
                .Include(i => i.ProductsNavigation)
                .Include(i => i.RawMaterialsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingridient == null)
            {
                return NotFound();
            }

            return View(ingridient);
        }

        // GET: Ingridients/Create
        public IActionResult Create()
        {
            ViewData["Products"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Name");
            return View();
        }

        // POST: Ingridients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Products,RawMaterials,Count")] Ingridient ingridient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingridient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Products"] = new SelectList(_context.FinishedProducts, "Id", "Name", ingridient.Products);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Name", ingridient.RawMaterials);
            return View(ingridient);
        }

        // GET: Ingridients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingridient = await _context.Ingridients.FindAsync(id);
            if (ingridient == null)
            {
                return NotFound();
            }
            ViewData["Products"] = new SelectList(_context.FinishedProducts, "Id", "Name", ingridient.Products);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Name", ingridient.RawMaterials);
            return View(ingridient);
        }

        // POST: Ingridients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Products,RawMaterials,Count")] Ingridient ingridient)
        {
            if (id != ingridient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingridient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngridientExists(ingridient.Id))
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
            ViewData["Products"] = new SelectList(_context.FinishedProducts, "Id", "Name", ingridient.Products);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Name", ingridient.RawMaterials);
            return View(ingridient);
        }

        // GET: Ingridients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingridient = await _context.Ingridients
                .Include(i => i.ProductsNavigation)
                .Include(i => i.RawMaterialsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingridient == null)
            {
                return NotFound();
            }

            return View(ingridient);
        }

        // POST: Ingridients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingridient = await _context.Ingridients.FindAsync(id);
            _context.Ingridients.Remove(ingridient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngridientExists(int id)
        {
            return _context.Ingridients.Any(e => e.Id == id);
        }
    }
}
