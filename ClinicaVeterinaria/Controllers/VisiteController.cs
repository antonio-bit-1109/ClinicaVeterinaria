using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            var socityPetContext = _context.Visites.Include(v => v.IdAnimaleNavigation).Include(v => v.IdRicettaNavigation);
            return View(await socityPetContext.ToListAsync());
        }

        // GET: Visite/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale");
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica");
            return View();
        }

        // POST: Visite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataVisita,Anamnesi,DescrizioneCura,IdAnimale,IdRicetta")] Visite visite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "Idanimale", visite.IdAnimale);
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "IdricettaMedica", visite.IdRicetta);
            return View(visite);
        }

        // GET: Visite/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("IdVisita,DataVisita,Anamnesi,DescrizioneCura,IdAnimale,IdRicetta")] Visite visite)
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
    }
}
