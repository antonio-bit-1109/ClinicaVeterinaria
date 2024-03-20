using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers
{
    public class VisiteController : Controller
    {
        private readonly SocityPetContext _context;

        public VisiteController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Visite
        //public async Task<IActionResult> Index()
        //{
        //    string navfoot = "vet";
        //    ViewBag.NavFoot = navfoot;
        //    string text = "bl";
        //    ViewBag.Text = text;

        //    var socityPetContext = _context.Visites.Include(v => v.IdAnimaleNavigation).Include(v => v.IdRicettaNavigation);
        //    return View(await socityPetContext.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var visite = await _context.Visites.Include(v => v.IdAnimaleNavigation).ToListAsync();
            return View(visite);
        }

        // GET: Visite/Details/5
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

            var visite = await _context.Visites
                .Include(v => v.IdAnimaleNavigation)
                .Include(v => v.IdRicettaNavigation)
                .FirstOrDefaultAsync(m => m.IdVisita == id);
            if (visite == null)
            {
                return NotFound();
            }

            return View(visite);
        }

        // GET: Visite/Create
        public IActionResult Create()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale");
            //ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica");
            ViewBag.ProdottoId = new SelectList(_context.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
            ViewBag.Ricetta = new Ricettemediche();
            return View();
        }

        // POST: Visite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataVisita,Anamnesi,DescrizioneCura,IdAnimale,IdRicetta,PrezzoVisita")] Visite visita, int[] prodottiId, bool aggiungiRicetta, string descrizioneRicetta)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Ricettemediche ricetta = null;
                        if (aggiungiRicetta)
                        {
                            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                            ricetta = new Ricettemediche
                            {
                                Descrizione = descrizioneRicetta,
                                IdUtente = userId,
                                DataPrescrizione = DateTime.Now
                            };
                            _context.Ricettemediches.Add(ricetta);
                            await _context.SaveChangesAsync();

                            visita.IdRicetta = ricetta.IdricettaMedica;
                        }

                        _context.Visites.Add(visita);
                        await _context.SaveChangesAsync();

                        if (aggiungiRicetta && prodottiId != null)
                        {
                            foreach (var prodottoId in prodottiId)
                            {
                                string insertSql = @"
                            INSERT INTO ProdottiRicette (IdProdotto, IdRicettaMedica)
                            VALUES ({0}, {1})";
                                await _context.Database.ExecuteSqlRawAsync(insertSql, prodottoId, ricetta.IdricettaMedica);
                            }
                        }

                        await transaction.CommitAsync();
                        TempData["Successo"] = "La visita è stata creata con successo.";
                        return RedirectToAction("Visita");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        TempData["Errore"] = "Si è verificato un errore: " + ex.Message;
                    }
                }
            }
            else
            {
                ViewData["IdAnimale"] = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
                ViewBag.ProdottoId = new SelectList(_context.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
                ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "Descrizione");
            }

            return View(visita);
        }

        // GET: Visite/Edit/5
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

            var visite = await _context.Visites.FindAsync(id);
            if (visite == null)
            {
                return NotFound();
            }
            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", visite.IdAnimale);
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", visite.IdRicetta);
            return View(visite);
        }

        // POST: Visite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVisita,DataVisita,Anamnesi,DescrizioneCura,IdAnimale,IdRicetta,PrezzoVisita")] Visite visite)
        {
            if (id != visite.IdVisita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisiteExists(visite.IdVisita))
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
            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", visite.IdAnimale);
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", visite.IdRicetta);
            return View(visite);
        }

        // GET: Visite/Delete/5
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

            var visite = await _context.Visites
                .Include(v => v.IdAnimaleNavigation)
                .Include(v => v.IdRicettaNavigation)
                .FirstOrDefaultAsync(m => m.IdVisita == id);
            if (visite == null)
            {
                return NotFound();
            }

            return View(visite);
        }

        // POST: Visite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visite = await _context.Visites.FindAsync(id);
            if (visite != null)
            {
                _context.Visites.Remove(visite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisiteExists(int id)
        {
            return _context.Visites.Any(e => e.IdVisita == id);
        }

        public IActionResult CronistoriaVisite(int id)
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var visiteAnimale = _context.Visites

                .Where(v => v.IdAnimale == id)
                .OrderByDescending(v => v.DataVisita)
                .ToList();

            return View(visiteAnimale);
        }
    }
}
