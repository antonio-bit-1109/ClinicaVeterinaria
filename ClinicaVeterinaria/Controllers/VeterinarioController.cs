using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        List<Animali> animali = _db.Animalis.ToList();
        List<Prodotti> prodottiMedicinali = _db.Prodottis.Where(p => p.IsMedicinale).ToList();

        if (!animali.Any())
        {
            TempData["Errore"] = "Non ci sono dati sugli animali disponibili.";
            return RedirectToAction("Index");
        }

        if (!prodottiMedicinali.Any())
        {
            TempData["Errore"] = "Non ci sono prodotti medicinali disponibili.";
            return RedirectToAction("Index");
        }

        ViewBag.IdAnimale = new SelectList(animali, "IdAnimale", "NomeAnimale");
        ViewBag.ProdottoId = new SelectList(prodottiMedicinali, "IdProdotto", "Nomeprodotto");
        ViewBag.IdRicetta = new SelectList(_db.Ricettemediches, "IdricettaMedica", "Descrizione"); // Assicurati che "Descrizione" sia il campo corretto

        return View();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVisita(Visite visita, Ricettemediche ricetta, int[] prodottiId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Logica di creazione della ricetta e della visita...
                _db.Add(visita);
                await _db.SaveChangesAsync();
                TempData["Successo"] = "La visita è stata creata con successo.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Logga l'eccezione qui con il tuo sistema di logging.
                TempData["Errore"] = "Si è verificato un errore durante la creazione della visita: " + ex.Message;
            }
        }
        else
        {
            // Se il modello non è valido, riempie di nuovo i dropdown
            ViewData["IdAnimale"] = new SelectList(_db.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
            ViewData["ProdottoId"] = new SelectList(_db.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
            ViewData["IdRicetta"] = new SelectList(_db.Ricettemediches, "IdricettaMedica", "Descrizione", ricetta.IdricettaMedica); // Cambia "Descrizione" se necessario
        }

        // Resta nella vista corrente mostrando gli errori di validazione del modello
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
