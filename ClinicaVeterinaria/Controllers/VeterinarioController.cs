using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Veterinario")]
public class VeterinarioController : Controller
{
    private readonly SocityPetContext _db;

    public VeterinarioController(SocityPetContext db)
    {
        _db = db;
    }

    // Mostra la lista delle visite
    public async Task<IActionResult> Index()
    {
        var visite = await _db.Visites.Include(v => v.IdAnimaleNavigation).ToListAsync();
        return View(visite);
    }

    // Mostra i dettagli di una specifica visita
    public async Task<IActionResult> Details(int id)
    {
        Visite? visita = await _db.Visites
                              .Include(v => v.IdAnimaleNavigation)
                              .Include(v => v.IdRicettaNavigation)
                              .FirstOrDefaultAsync(v => v.IdVisita == id);

        if (visita == null)
        {
            return NotFound();
        }

        return View(visita);
    }

    // GET: Veterinario/Create
    public IActionResult Visita()
    {
        try
        {
            //System.Diagnostics.Debug.WriteLine("Prima di inizializzare ViewData['IdAnimale']");

            ViewData["IdAnimale"] = new SelectList(_db.Animalis, "IdAnimale", "NomeAnimale");

            //System.Diagnostics.Debug.WriteLine("ViewData['IdAnimale'] inizializzato");

            //System.Diagnostics.Debug.WriteLine("Prima di inizializzare ViewBag.ProdottoId");

            ViewBag.ProdottoId = new SelectList(_db.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");

            //System.Diagnostics.Debug.WriteLine("ViewBag.ProdottoId inizializzato");

            //System.Diagnostics.Debug.WriteLine("Prima di inizializzare ViewBag.Ricetta");

            ViewBag.Ricetta = new Ricettemediche();

            //System.Diagnostics.Debug.WriteLine("ViewBag.Ricetta inizializzato");

            return View();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Errore catturato: " + ex.ToString());
            ViewData["ErrorMessage"] = "Si è verificato un errore: " + ex.Message;
            return View("Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVisita(Visite visita, int[] prodottiId, bool aggiungiRicetta, string descrizioneRicetta)
    {
        if (ModelState.IsValid)
        {
            using (var transaction = _db.Database.BeginTransaction())
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
                        _db.Ricettemediches.Add(ricetta);
                        await _db.SaveChangesAsync();

                        visita.IdRicetta = ricetta.IdricettaMedica;
                    }

                    _db.Visites.Add(visita);
                    await _db.SaveChangesAsync();

                    if (aggiungiRicetta && prodottiId != null)
                    {
                        foreach (var prodottoId in prodottiId)
                        {
                            string insertSql = @"
                            INSERT INTO ProdottiRicette (IdProdotto, IdRicettaMedica)
                            VALUES ({0}, {1})";
                            await _db.Database.ExecuteSqlRawAsync(insertSql, prodottoId, ricetta.IdricettaMedica);
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
            ViewData["IdAnimale"] = new SelectList(_db.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
            ViewBag.ProdottoId = new SelectList(_db.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
            ViewData["IdRicetta"] = new SelectList(_db.Ricettemediches, "IdricettaMedica", "Descrizione");
        }

        return View(visita);
    }

    // GET: Veterinario/Edit/5
    public async Task<IActionResult> EditVisita(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Visite? visita = await _db.Visites.FindAsync(id);
        if (visita == null)
        {
            return NotFound();
        }
        ViewData["IdAnimale"] = new SelectList(_db.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
        return View(visita);
    }

    // POST: Veterinario/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditVisita(int id, Visite visita)
    {
        if (id != visita.IdVisita)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(visita);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitaExists(visita.IdVisita))
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
        ViewData["AnimaliId"] = new SelectList(_db.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
        return View(visita);
    }

    // GET: Veterinario/Delete/5
    public async Task<IActionResult> DeleteVisita(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Visite? visita = await _db.Visites
            .Include(v => v.IdAnimaleNavigation)
            .FirstOrDefaultAsync(m => m.IdVisita == id);
        if (visita == null)
        {
            return NotFound();
        }

        return View(visita);
    }

    // POST: Veterinario/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Visite? visita = await _db.Visites.FindAsync(id);
        _db.Visites.Remove(visita);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VisitaExists(int id)
    {
        return _db.Visites.Any(e => e.IdVisita == id);
    }

}
