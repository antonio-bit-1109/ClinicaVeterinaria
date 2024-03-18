using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers
{
    public class LoginController : Controller
    {

        private readonly SocityPetContext _db;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        public LoginController(SocityPetContext db, IAuthenticationSchemeProvider schemeProvider)
        {
            _db = db;
            _schemeProvider = schemeProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Utenti utente)
        {
            if (!string.IsNullOrEmpty(utente.Nome) && !string.IsNullOrEmpty(utente.Password))
            {
                var user = await _db.Utentis
                                         .Include(u => u.IdRuoloNavigation) // Include l'entità Ruoli
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(u => u.Nome == utente.Nome);

                if (user != null && VerifyPasswordHash(utente.Password, user.Password))
                {
                    var roleName = user.IdRuoloNavigation?.NomeRuolo ?? "RuoloNonDefinito"; // Assegna un valore predefinito se il ruolo non è trovato
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome), // nome
                new Claim(ClaimTypes.Role, roleName), // Usa il nome del ruolo qui
                new Claim(ClaimTypes.NameIdentifier, user.IdUtente.ToString()) // id
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity),
                       authProperties);

                    TempData["Message"] = "Login effettuato con successo";
                    return RedirectToAction("Index");
                }
                TempData["Errore"] = "Nome utente o password non corretti.";
                return RedirectToAction("Index", "Home");
            }
            TempData["Errore"] = "Username e password sono richiesti.";
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Registrazione()
        {
            bool MiStoRegistrando;


            MiStoRegistrando = true;
            TempData["MiStoRegistrando"] = MiStoRegistrando;
            TempData["infoRegistrazione"] = "Inserisci qui sotto i tuoi dati di registrazione.";
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> ConcludiRegistrazione(Utenti utente)
        {
            if (ModelState.IsValid)
            {
                // Controlla se l'utente esiste già
                var existingUser = await _db.Utentis
                                            .AsNoTracking()
                                            .AnyAsync(u => u.Nome == utente.Nome);
                if (existingUser)
                {
                    TempData["Errore"] = "Un utente con questo nome esiste già.";
                    return RedirectToAction("Index", "Home");
                }

                utente.IdRuolo = 1; // Ruolo utente
                // Hash della password
                utente.Password = HashPassword(utente.Password);

                // Salva l'utente nel database
                _db.Utentis.Add(utente);
                await _db.SaveChangesAsync();

                TempData["Message"] = "Registrazione effettuata con successo.";
                return RedirectToAction("Index", "Home");
            }
            // Se il modello non è valido, ritorna alla vista di registrazione con i messaggi di errore.
            return View("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Message"] = "Logout effettuato.";
            return RedirectToAction("Index", "Home");
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
