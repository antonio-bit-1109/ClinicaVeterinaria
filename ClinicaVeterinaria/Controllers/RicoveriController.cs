﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
    public class RicoveriController : Controller
    {
        private readonly SocityPetContext _context;

        public RicoveriController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Ricoveri
        public async Task<IActionResult> Index()
        {
            var socityPetContext = _context.Ricoveris.Include(r => r.IdanimaleNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: Ricoveri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ricoveri = await _context.Ricoveris
                .Include(r => r.IdanimaleNavigation)
                .FirstOrDefaultAsync(m => m.IdRicovero == id);
            if (ricoveri == null)
            {
                return NotFound();
            }

            return View(ricoveri);
        }

        // GET: Ricoveri/Create
        public IActionResult Create()
        {
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale");
            return View();
        }

        // POST: Ricoveri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRicovero,Dataregistrazionericovero,Idanimale,DataInizioRicovero,DataFinericovero,PrezzoGiornalieroRicovero,IsRicoveroAttivo,PrezzoTotaleRicovero")] Ricoveri ricoveri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ricoveri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", ricoveri.Idanimale);
            return View(ricoveri);
        }

        // GET: Ricoveri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ricoveri = await _context.Ricoveris.FindAsync(id);
            if (ricoveri == null)
            {
                return NotFound();
            }
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", ricoveri.Idanimale);
            return View(ricoveri);
        }

        // POST: Ricoveri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRicovero,Dataregistrazionericovero,Idanimale,DataInizioRicovero,DataFinericovero,PrezzoGiornalieroRicovero,IsRicoveroAttivo,PrezzoTotaleRicovero")] Ricoveri ricoveri)
        {
            if (id != ricoveri.IdRicovero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ricoveri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RicoveriExists(ricoveri.IdRicovero))
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
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", ricoveri.Idanimale);
            return View(ricoveri);
        }

        // GET: Ricoveri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ricoveri = await _context.Ricoveris
                .Include(r => r.IdanimaleNavigation)
                .FirstOrDefaultAsync(m => m.IdRicovero == id);
            if (ricoveri == null)
            {
                return NotFound();
            }

            return View(ricoveri);
        }

        // POST: Ricoveri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ricoveri = await _context.Ricoveris.FindAsync(id);
            if (ricoveri != null)
            {
                _context.Ricoveris.Remove(ricoveri);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RicoveriExists(int id)
        {
            return _context.Ricoveris.Any(e => e.IdRicovero == id);
        }
    }
}