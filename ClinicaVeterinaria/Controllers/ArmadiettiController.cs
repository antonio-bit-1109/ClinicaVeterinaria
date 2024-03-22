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
    public class ArmadiettiController : Controller
    {
        private readonly SocityPetContext _context;

        public ArmadiettiController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Armadietti
        public async Task<IActionResult> Index()
        {
            return View(await _context.Armadiettis.ToListAsync());
        }

        // GET: Armadietti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armadietti = await _context.Armadiettis
                .FirstOrDefaultAsync(m => m.IdArmadietto == id);
            if (armadietti == null)
            {
                return NotFound();
            }

            return View(armadietti);
        }

        // GET: Armadietti/Create
        public IActionResult Create()
        {
            return View();
        }


       



        // POST: Armadietti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArmadietto,Descrizione")] Armadietti armadietti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(armadietti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(armadietti);
        }

        // GET: Armadietti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armadietti = await _context.Armadiettis.FindAsync(id);
            if (armadietti == null)
            {
                return NotFound();
            }
            return View(armadietti);
        }

        // POST: Armadietti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArmadietto,Descrizione")] Armadietti armadietti)
        {
            if (id != armadietti.IdArmadietto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(armadietti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArmadiettiExists(armadietti.IdArmadietto))
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
            return View(armadietti);
        }

        // GET: Armadietti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var armadietti = await _context.Armadiettis
                .FirstOrDefaultAsync(m => m.IdArmadietto == id);
            if (armadietti == null)
            {
                return NotFound();
            }

            return View(armadietti);
        }

        // POST: Armadietti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var armadietti = await _context.Armadiettis.FindAsync(id);
            if (armadietti != null)
            {
                _context.Armadiettis.Remove(armadietti);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArmadiettiExists(int id)
        {
            return _context.Armadiettis.Any(e => e.IdArmadietto == id);
        }
    }
}
