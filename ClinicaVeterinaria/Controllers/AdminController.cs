// In un nuovo controller, ad esempio AdminController.cs
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


public class AdminController : Controller
{
    private readonly SocityPetContext _db;

    public AdminController(SocityPetContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> BackOffice()
    {
        string navfoot = "ad";
        ViewBag.NavFoot = navfoot;
        string text = "wh";
        ViewBag.Text = text;

        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //var utenti = await _db.Utentis.Include(u => u.IdRuoloNavigation).ToListAsync();

        //// Debug: stampa il numero di utenti recuperati
        //System.Diagnostics.Debug.WriteLine($"Utenti recuperati: {utenti.Count}");

        var viewModel = new BackOfficeModel
        {
            Utenti = await _db.Utentis.Include(u => u.IdRuoloNavigation).ToListAsync(),
            Ruoli = await _db.Ruolis.ToListAsync(),
            CurrentUserId = currentUserId,
            Animali = await _db.Animalis.ToListAsync(),
            Armadietti = await _db.Armadiettis.ToListAsync(),
            Cassetti = await _db.Cassettis.ToListAsync(),
            Dittafornitrice = await _db.Dittafornitrices.ToListAsync(),
            Prodotti = await _db.Prodottis.ToListAsync(),
            ProdottiInCassetto = await _db.ProdottiInCassettos.ToListAsync(),
            Ricettemediche = await _db.Ricettemediches.ToListAsync(),
            Ricoveri = await _db.Ricoveris.ToListAsync(),
            Visite = await _db.Visites.ToListAsync(),
            Vendite = await _db.Vendites.ToListAsync(),
        };

        return View(viewModel);
    }

    // metodo per cambiare il ruolo di un utente (ad esempio da "User" a "Admin") 
    // solo da parte di un utente con ruolo "Admin" e non può cambiare il proprio ruolo
    [HttpPost]
    public async Task<IActionResult> ChangeUserRole(int idUtente, int nuovoIdRuolo)
    {
        string navfoot = "ad";
        ViewBag.NavFoot = navfoot;
        string text = "wh";
        ViewBag.Text = text;

        if (!ModelState.IsValid)
        {
            // Se il modello non è valido, potresti voler ritornare alla stessa view con un messaggio di errore
            TempData["Errore"] = "Errore nel form.";
            return RedirectToAction(nameof(BackOffice));
        }

        // Verifica se l'utente corrente sta cercando di cambiare il proprio ruolo
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId == idUtente.ToString())
        {
            TempData["Error"] = "Non è consentito modificare il proprio ruolo.";
            return RedirectToAction(nameof(BackOffice));
        }

        // Trova l'utente nel database
        var utenteDaModificare = await _db.Utentis.FindAsync(idUtente);
        if (utenteDaModificare == null)
        {
            // Se non trovi l'utente, ritorna un errore
            TempData["Errore"] = "Utente non trovato.";
            return RedirectToAction(nameof(BackOffice));
        }

        // Assicurati che il nuovo ruolo esista
        var ruoloEsiste = await _db.Ruolis.AnyAsync(r => r.IdRuolo == nuovoIdRuolo);
        if (!ruoloEsiste)
        {
            // Se il ruolo non esiste, ritorna un errore
            TempData["Errore"] = "Il ruolo specificato non esiste.";
            return RedirectToAction(nameof(BackOffice));
        }

        // Aggiorna il ruolo dell'utente
        utenteDaModificare.IdRuolo = nuovoIdRuolo;
        await _db.SaveChangesAsync();
        TempData["Successo"] = "Ruolo utente aggiornato con successo.";

        return RedirectToAction(nameof(BackOffice));
    }

    // Aggiungere altre azioni come Edit, Delete ecc...
}
