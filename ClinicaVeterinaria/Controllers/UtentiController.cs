using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers
{
    public class UtentiController : Controller
    {
        private readonly SocityPetContext _context;

        public UtentiController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Utenti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string navfoot = "ad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utentis
                .Include(u => u.IdRuoloNavigation)
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (utenti == null)
            {
                return NotFound();
            }

            return View(utenti);
        }


        // GET: Utenti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string navfoot = "ad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utentis.FindAsync(id);
            if (utenti == null)
            {
                return NotFound();
            }
            ViewData["IdRuolo"] = new SelectList(_context.Ruolis, "IdRuolo", "NomeRuolo", utenti.IdRuolo);
            return View(utenti);
        }

        // POST: Utenti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUtente,Nome,Cognome")] Utenti utenti, string newPassword, string confirmNewPassword, IFormFile uploadedImage)
        {
            ModelState.Remove("Password");
            if (id != utenti.IdUtente)
            {
                return NotFound();
            }


            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError("confirmNewPassword", "La nuova password e la conferma non corrispondono.");
            }

            if (ModelState.IsValid)
            {
                if (uploadedImage != null && uploadedImage.Length > 0)
                {
                    var fileName = Path.GetFileName(uploadedImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/utenti", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedImage.CopyToAsync(fileStream);
                    }

                    utenti.FotoUtente = fileName; // Aggiorna il percorso della foto solo se una nuova è stata caricata
                }


                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    // Se è stata inserita una nuova password e confermata correttamente, criptala
                    utenti.Password = HashPassword(newPassword);
                }
                else
                {
                    // Se non è stata inserita una nuova password, non aggiornare il campo password
                    _context.Entry(utenti).Property("Password").IsModified = false;
                }

                try
                {
                    _context.Update(utenti);
                    await _context.SaveChangesAsync();
                    var nome = User.FindFirst(ClaimTypes.Name)?.Value;
                    TempData["Successo"] = $"Dati di {nome} aggiornati con successo.";
                    return RedirectToAction(nameof(Details), new { id = utenti.IdUtente });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtentiExists(utenti.IdUtente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Prepara di nuovo la vista in caso di errori
            ViewData["IdRuolo"] = new SelectList(_context.Ruolis, "IdRuolo", "NomeRuolo", utenti.IdRuolo);
            return View(utenti);
        }

        // GET: Utenti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string navfoot = "ad";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utentis
                .Include(u => u.IdRuoloNavigation)
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (utenti == null)
            {
                return NotFound();
            }

            return View(utenti);
        }

        // POST: Utenti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utenti = await _context.Utentis.FindAsync(id);
            if (utenti != null)
            {
                _context.Utentis.Remove(utenti);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        // questo metodo fa lhas della password
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool UtentiExists(int id)
        {
            return _context.Utentis.Any(e => e.IdUtente == id);
        }
    }
}
