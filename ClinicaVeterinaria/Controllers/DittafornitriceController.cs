using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
    public class DittafornitriceController : Controller
    {
        private readonly SocityPetContext _context;

        public DittafornitriceController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Dittafornitrice
        public async Task<IActionResult> Index()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            return View(await _context.Dittafornitrices.ToListAsync());
        }

        // GET: Dittafornitrice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var dittafornitrice = await _context.Dittafornitrices
                .FirstOrDefaultAsync(m => m.IdDittaFornitrice == id);
            if (dittafornitrice == null)
            {
                return NotFound();
            }

            return View(dittafornitrice);
        }

        // GET: Dittafornitrice/Create
        public IActionResult Create()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            return View();
        }

        // POST: Dittafornitrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDittaFornitrice,NomeDitta,RecapitoDitta,Indirizzo")] Dittafornitrice dittafornitrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dittafornitrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dittafornitrice);
        }

        // GET: Dittafornitrice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var dittafornitrice = await _context.Dittafornitrices.FindAsync(id);
            if (dittafornitrice == null)
            {
                return NotFound();
            }
            return View(dittafornitrice);
        }

        // POST: Dittafornitrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDittaFornitrice,NomeDitta,RecapitoDitta,Indirizzo")] Dittafornitrice dittafornitrice)
        {
            if (id != dittafornitrice.IdDittaFornitrice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dittafornitrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DittafornitriceExists(dittafornitrice.IdDittaFornitrice))
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
            return View(dittafornitrice);
        }

        // GET: Dittafornitrice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var dittafornitrice = await _context.Dittafornitrices
                .FirstOrDefaultAsync(m => m.IdDittaFornitrice == id);
            if (dittafornitrice == null)
            {
                return NotFound();
            }

            return View(dittafornitrice);
        }

        // POST: Dittafornitrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dittafornitrice = await _context.Dittafornitrices.FindAsync(id);
            if (dittafornitrice != null)
            {
                _context.Dittafornitrices.Remove(dittafornitrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DittafornitriceExists(int id)
        {
            return _context.Dittafornitrices.Any(e => e.IdDittaFornitrice == id);
        }
    }
}
