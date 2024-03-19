using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers
{
    public class RicettemedicheController : Controller
    {
        private readonly SocityPetContext _context;

        public RicettemedicheController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: Ricettemediche
        public async Task<IActionResult> Index()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            var socityPetContext = _context.Ricettemediches.Include(r => r.IdUtenteNavigation).Include(r => r.IdVisitaNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: Ricettemediche/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var ricettemediche = await _context.Ricettemediches
                .Include(r => r.IdUtenteNavigation)
                .Include(r => r.IdVisitaNavigation)
                .FirstOrDefaultAsync(m => m.IdricettaMedica == id);
            if (ricettemediche == null)
            {
                return NotFound();
            }

            return View(ricettemediche);
        }

        // GET: Ricettemediche/Create
        public IActionResult Create()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "Nome");
            ViewData["IdVisita"] = new SelectList(_context.Visites, "IdVisita", "IdVisita");
            return View();
        }

        // POST: Ricettemediche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVisita,IdUtente,DataPrescrizione,Descrizione")] Ricettemediche ricettemediche)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ricettemediche);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUtente"] = new SelectList(_context.Utentis.Where(u => u.IdRuoloNavigation.NomeRuolo == "Veterinario"), "IdUtente", "Nome", ricettemediche.IdUtente);
            ViewData["IdVisita"] = new SelectList(_context.Visites, "IdVisita", "IdVisita", ricettemediche.IdVisita);
            return View(ricettemediche);
        }

        // GET: Ricettemediche/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var ricettemediche = await _context.Ricettemediches.FindAsync(id);
            if (ricettemediche == null)
            {
                return NotFound();
            }
            ViewData["IdUtente"] = new SelectList(_context.Utentis.Where(u => u.IdRuoloNavigation.NomeRuolo == "Veterinario"), "IdUtente", "Nome", ricettemediche.IdUtente);
            ViewData["IdVisita"] = new SelectList(_context.Visites, "IdVisita", "IdVisita", ricettemediche.IdVisita);
            return View(ricettemediche);
        }

        // POST: Ricettemediche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdricettaMedica,IdVisita,IdUtente,DataPrescrizione,Descrizione")] Ricettemediche ricettemediche)
        {
            if (id != ricettemediche.IdricettaMedica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ricettemediche);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RicettemedicheExists(ricettemediche.IdricettaMedica))
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
            ViewData["IdUtente"] = new SelectList(_context.Utentis, "IdUtente", "IdUtente", ricettemediche.IdUtente);
            ViewData["IdVisita"] = new SelectList(_context.Visites, "IdVisita", "IdVisita", ricettemediche.IdVisita);
            return View(ricettemediche);
        }

        // GET: Ricettemediche/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (id == null)
            {
                return NotFound();
            }

            var ricettemediche = await _context.Ricettemediches
                .Include(r => r.IdUtenteNavigation)
                .Include(r => r.IdVisitaNavigation)
                .FirstOrDefaultAsync(m => m.IdricettaMedica == id);
            if (ricettemediche == null)
            {
                return NotFound();
            }

            return View(ricettemediche);
        }

        // POST: Ricettemediche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ricettemediche = await _context.Ricettemediches.FindAsync(id);
            if (ricettemediche != null)
            {
                _context.Ricettemediches.Remove(ricettemediche);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RicettemedicheExists(int id)
        {
            return _context.Ricettemediches.Any(e => e.IdricettaMedica == id);
        }
    }
}
