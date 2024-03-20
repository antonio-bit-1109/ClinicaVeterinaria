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
    public class CassettisController : Controller
    {
        private readonly SocityPetContext _context;

        public CassettisController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Cassettis
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                // Se l'ID non è stato fornito, restituisci tutti i cassetti
                var socityPetContext = _context.Cassettis.Include(c => c.IdArmadiettoNavigation);
                return View(await socityPetContext.ToListAsync());
            }
            else if (id == 1)
            {
                var socityPetContext = _context.Cassettis.Where(c => c.IdCassetto <= 7);
                return View(await socityPetContext.ToListAsync());
            }
            else if (id == 2)
            {
                var socityPetContext = _context.Cassettis.Where(c => c.IdCassetto > 7 && c.IdCassetto < 15);
                return View(await socityPetContext.ToListAsync());
            }
            else if (id == 3)
            {
                var socityPetContext = _context.Cassettis.Where(c => c.IdCassetto >= 15 && c.IdCassetto < 23);
                return View(await socityPetContext.ToListAsync());
            }
            else if (id == 4)
            {
                var socityPetContext = _context.Cassettis.Where(c => c.IdCassetto >= 23 );
                return View(await socityPetContext.ToListAsync());
            }

            // Aggiungi un'istruzione di ritorno di default per gestire tutti gli altri casi
            return NotFound();
        }

        // GET: Cassettis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cassetti = await _context.Cassettis
                .Include(c => c.IdArmadiettoNavigation)
                .FirstOrDefaultAsync(m => m.IdCassetto == id);
            if (cassetti == null)
            {
                return NotFound();
            }

            return View(cassetti);
        }

        // GET: Cassettis/Create
        public IActionResult Create()
        {
            ViewData["IdArmadietto"] = new SelectList(_context.Armadiettis, "IdArmadietto", "IdArmadietto");
            return View();
        }

        // POST: Cassettis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCassetto,IdArmadietto,Descrizione")] Cassetti cassetti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cassetti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArmadietto"] = new SelectList(_context.Armadiettis, "IdArmadietto", "IdArmadietto", cassetti.IdArmadietto);
            return View(cassetti);
        }

        // GET: Cassettis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cassetti = await _context.Cassettis.FindAsync(id);
            if (cassetti == null)
            {
                return NotFound();
            }
            ViewData["IdArmadietto"] = new SelectList(_context.Armadiettis, "IdArmadietto", "IdArmadietto", cassetti.IdArmadietto);
            return View(cassetti);
        }

        // POST: Cassettis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCassetto,IdArmadietto,Descrizione")] Cassetti cassetti)
        {
            if (id != cassetti.IdCassetto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cassetti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CassettiExists(cassetti.IdCassetto))
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
            ViewData["IdArmadietto"] = new SelectList(_context.Armadiettis, "IdArmadietto", "IdArmadietto", cassetti.IdArmadietto);
            return View(cassetti);
        }

        // GET: Cassettis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cassetti = await _context.Cassettis
                .Include(c => c.IdArmadiettoNavigation)
                .FirstOrDefaultAsync(m => m.IdCassetto == id);
            if (cassetti == null)
            {
                return NotFound();
            }

            return View(cassetti);
        }

        // POST: Cassettis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cassetti = await _context.Cassettis.FindAsync(id);
            if (cassetti != null)
            {
                _context.Cassettis.Remove(cassetti);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CassettiExists(int id)
        {
            return _context.Cassettis.Any(e => e.IdCassetto == id);
        }
    }
}
