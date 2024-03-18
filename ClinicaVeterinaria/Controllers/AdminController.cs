// In un nuovo controller, ad esempio AdminController.cs
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly SocityPetContext _db;

    public AdminController(SocityPetContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> BackOffice()
    {
        var viewModel = new BackOfficeModel
        {
            Utenti = await _db.Utentis.Include(u => u.IdRuoloNavigation).ToListAsync(),
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
            Ruoli = await _db.Ruolis.ToListAsync()
        };

        return View(viewModel);
    }

    // metodo per cambiare il ruolo di un utente (ad esempio da "User" a "Admin") 
    // solo da parte di un utente con ruolo "Admin" e non può cambiare il proprio ruolo
    [HttpPost]
    public async Task<IActionResult> ChangeUserRole(int idUtente, int nuovoIdRuolo)
    {
        var utente = await _db.Utentis.FindAsync(idUtente);
        if (utente == null)
        {
            return NotFound();
        }

        // Impedisci all'admin di modificare il proprio ruolo
        if (User.Identity.Name == utente.Nome)
        {
            return Forbid();
        }

        utente.IdRuolo = nuovoIdRuolo;
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(BackOffice));
    }

    // Aggiungere altre azioni come Edit, Delete ecc...
}
