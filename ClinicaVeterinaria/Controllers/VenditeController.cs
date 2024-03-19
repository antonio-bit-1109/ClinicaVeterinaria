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
    public class VenditeController : Controller
    {
        private readonly SocityPetContext _context;

        public VenditeController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Vendite
        public async Task<IActionResult> Index()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            var socityPetContext = _context.Vendites.Include(v => v.IdProdottoNavigation).Include(v => v.IdUtenteNavigation).Include(v => v.IdricettaMedicaNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: Vendite/Details/5
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

            var vendite = await _context.Vendites
                .Include(v => v.IdProdottoNavigation)
                .Include(v => v.IdUtenteNavigation)
                .Include(v => v.IdricettaMedicaNavigation)
                .FirstOrDefaultAsync(m => m.IdVendita == id);
            if (vendite == null)
            {
                return NotFound();
            }

            return View(vendite);
        }

        // GET: Vendite/Create
        public IActionResult Create()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto");
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente");
            ViewData["IdricettaMedica"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica");
            return View();
        }

        // POST: Vendite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVendita,IdProdotto,IdUtente,Cf,IdricettaMedica")] Vendite vendite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", vendite.IdProdotto);
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente", vendite.IdUtente);
            ViewData["IdricettaMedica"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", vendite.IdricettaMedica);
            return View(vendite);
        }

        // GET: Vendite/Edit/5
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

            var vendite = await _context.Vendites.FindAsync(id);
            if (vendite == null)
            {
                return NotFound();
            }
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", vendite.IdProdotto);
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente", vendite.IdUtente);
            ViewData["IdricettaMedica"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", vendite.IdricettaMedica);
            return View(vendite);
        }

        // POST: Vendite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVendita,IdProdotto,IdUtente,Cf,IdricettaMedica")] Vendite vendite)
        {
            if (id != vendite.IdVendita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenditeExists(vendite.IdVendita))
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
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", vendite.IdProdotto);
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente", vendite.IdUtente);
            ViewData["IdricettaMedica"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", vendite.IdricettaMedica);
            return View(vendite);
        }

        // GET: Vendite/Delete/5
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

            var vendite = await _context.Vendites
                .Include(v => v.IdProdottoNavigation)
                .Include(v => v.IdUtenteNavigation)
                .Include(v => v.IdricettaMedicaNavigation)
                .FirstOrDefaultAsync(m => m.IdVendita == id);
            if (vendite == null)
            {
                return NotFound();
            }

            return View(vendite);
        }

        // POST: Vendite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendite = await _context.Vendites.FindAsync(id);
            if (vendite != null)
            {
                _context.Vendites.Remove(vendite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenditeExists(int id)
        {
            return _context.Vendites.Any(e => e.IdVendita == id);
        }
    }
}
