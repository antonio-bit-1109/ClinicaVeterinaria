﻿using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers
{
    public class AnimaliController : Controller
    {
        private readonly SocityPetContext _context;

        public AnimaliController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Animali
        public async Task<IActionResult> Index()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var socityPetContext = _context.Animalis.Include(a => a.IdUtenteNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: Animali/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var animali = await _context.Animalis
                .Include(a => a.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdAnimale == id);
            if (animali == null)
            {
                return NotFound();
            }

            return View(animali);
        }

        // GET: Animali/Create
        [HttpGet]
        public IActionResult Create()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            ViewData["IdUtente"] = new SelectList(_context.Utentis.Where(u => u.IdRuolo != 4).Select(u => new { u.IdUtente, NomeCompleto = u.Nome + " " + u.Cognome }), "IdUtente", "NomeCompleto");
            return View();
        }

        // POST: Animali/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dataregistrazione,NomeAnimale,Tipologia,ColoreMantello," +
            "Datanascita,HasMicrochip,NumMicrochip,HasProprietario,IdUtente")] Animali animali, IFormFile FotoAnimale)
        {
            ModelState.Remove("IdUtenteNavigation");
            ModelState.Remove("Ricoveris");
            ModelState.Remove("Visites");


            if (ModelState.IsValid)
            {
                if (FotoAnimale != null && FotoAnimale.Length > 0)
                {
                    var fileName = Path.GetFileName(FotoAnimale.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/animali", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoAnimale.CopyToAsync(fileStream);
                    }

                    animali.FotoAnimale = fileName;
                }

                _context.Add(animali);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "Nome", animali.IdUtente);
            return RedirectToAction("Index");
        }

        // GET: Animali/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var animali = await _context.Animalis.FindAsync(id);
            if (animali == null)
            {
                return NotFound();
            }

            ViewData["IdUtente"] = new SelectList(_context.Utentis.Where(u => u.IdRuolo != 4).Select(u => new { u.IdUtente, NomeCompleto = u.Nome + " " + u.Cognome }), "IdUtente", "NomeCompleto");
            //ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "Nome", animali.IdUtente);
            return View(animali);
        }

        // POST: Animali/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idanimale,Dataregistrazione,NomeAnimale,Tipologia," +
            "ColoreMantello,Datanascita,HasMicrochip,NumMicrochip,HasProprietario,IdUtente")] Animali animale, IFormFile? FotoAnimale)
        {
            if (id != animale.IdAnimale)
            {
                return NotFound();
            }

            ModelState.Remove("IdUtenteNavigation");
            ModelState.Remove("Ricoveris");
            ModelState.Remove("Visites");

            if (ModelState.IsValid)
            {
                try
                {
                    if (FotoAnimale != null && FotoAnimale.Length > 0)
                    {
                        var fileName = Path.GetFileName(FotoAnimale.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/animali", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FotoAnimale.CopyToAsync(fileStream);
                        }

                        animale.FotoAnimale = fileName;
                    }
                    else
                    {
                        // Se FotoAnimale è null, manteniamo l'immagine esistente.
                        var animaliEsistente = await _context.Animalis.AsNoTracking().FirstOrDefaultAsync(a => a.IdAnimale == id);
                        animale.FotoAnimale = animaliEsistente?.FotoAnimale;
                    }

                    _context.Update(animale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimaliExists(animale.IdAnimale))
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
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente", animale.IdUtente);
            return View(animale);
        }

        // GET: Animali/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var animali = await _context.Animalis
                .Include(a => a.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdAnimale == id);
            if (animali == null)
            {
                return NotFound();
            }

            return View(animali);
        }

        // POST: Animali/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animali = await _context.Animalis.FindAsync(id);
            if (animali != null)
            {
                _context.Animalis.Remove(animali);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimaliExists(int id)
        {
            return _context.Animalis.Any(e => e.IdAnimale == id);
        }


        public IActionResult getAnimalByMicrochip(string stringMicro)
        {
            var AnimaleByMicrochip = _context.Animalis.Where(t => t.NumMicrochip == stringMicro).FirstOrDefault();

            return Json(AnimaleByMicrochip);
        }

        public IActionResult IsAnimaleRicoverato(int idAnimale)
        {
            //var isAnimaleRicoverato = _context.Ricoveris.Where(r => r.IdAnimale == idAnimale && r.IsRicoveroAttivo == true).FirstOrDefault();
            var animaleRicoverato = _context.Ricoveris.Where(r => r.IdAnimale == idAnimale).Where(r => r.IsRicoveroAttivo == Convert.ToBoolean(1)).FirstOrDefault();

            return Json(animaleRicoverato);
        }
    }
}
