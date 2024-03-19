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
    public class ProdottiInCassettoesController : Controller
    {
        private readonly SocityPetContext _context;

        public ProdottiInCassettoesController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: ProdottiInCassettoes
        public async Task<IActionResult> Index()
        {
            var socityPetContext = _context.ProdottiInCassettos.Include(p => p.IdCassettoNavigation.Descrizione).Include(p => p.IdProdottoNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: ProdottiInCassettoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodottiInCassetto = await _context.ProdottiInCassettos
                .Include(p => p.IdCassettoNavigation)
                .Include(p => p.IdProdottoNavigation)
                .FirstOrDefaultAsync(m => m.IdProdottoInCassetto == id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }

            return View(prodottiInCassetto);
        }

        // GET: ProdottiInCassettoes/Create
        public IActionResult Create()
        {
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "Descrizione");
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "Nomeprodotto");
            return View();
        }

        // POST: ProdottiInCassettoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProdottoInCassetto,IdProdotto,IdCassetto,Quantita")] ProdottiInCassetto prodottiInCassetto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prodottiInCassetto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "Descrizione", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "Nomeprodotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }

        // GET: ProdottiInCassettoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodottiInCassetto = await _context.ProdottiInCassettos.FindAsync(id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "IdCassetto", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }

        // POST: ProdottiInCassettoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProdottoInCassetto,IdProdotto,IdCassetto,Quantita")] ProdottiInCassetto prodottiInCassetto)
        {
            if (id != prodottiInCassetto.IdProdottoInCassetto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodottiInCassetto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdottiInCassettoExists(prodottiInCassetto.IdProdottoInCassetto))
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
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "IdCassetto", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }

        // GET: ProdottiInCassettoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodottiInCassetto = await _context.ProdottiInCassettos
                .Include(p => p.IdCassettoNavigation)
                .Include(p => p.IdProdottoNavigation)
                .FirstOrDefaultAsync(m => m.IdProdottoInCassetto == id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }

            return View(prodottiInCassetto);
        }

        // POST: ProdottiInCassettoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prodottiInCassetto = await _context.ProdottiInCassettos.FindAsync(id);
            if (prodottiInCassetto != null)
            {
                _context.ProdottiInCassettos.Remove(prodottiInCassetto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdottiInCassettoExists(int id)
        {
            return _context.ProdottiInCassettos.Any(e => e.IdProdottoInCassetto == id);
        }
    }
}
