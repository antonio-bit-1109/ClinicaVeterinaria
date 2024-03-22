using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Controllers
{
	[Authorize(Roles = "Veterinario , Admin")]
	public class RicoveriController : Controller
	{
		private readonly SocityPetContext _context;

		public RicoveriController(SocityPetContext context)
		{
			_context = context;
		}

		// GET: Ricoveri
		public async Task<IActionResult> Index()
		{
			string navfoot = "vet";
			ViewBag.NavFoot = navfoot;
			string text = "bl";
			ViewBag.Text = text;

			var socityPetContext = _context.Ricoveris.Include(r => r.IdAnimaleNavigation);
			return View(await socityPetContext.ToListAsync());
		}

		// GET: Ricoveri/Details/5
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

			var ricoveri = await _context.Ricoveris
				.Include(r => r.IdAnimaleNavigation)
				.FirstOrDefaultAsync(m => m.IdRicovero == id);
			if (ricoveri == null)
			{
				return NotFound();
			}

			return View(ricoveri);
		}

		// GET: Ricoveri/Create
		public IActionResult Create()
		{
			string navfoot = "vet";
			ViewBag.NavFoot = navfoot;
			string text = "bl";
			ViewBag.Text = text;

            ViewBag.Idanimale = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale");
            return View();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Ricoveri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAnimale,PrezzoGiornalieroRicovero")] Ricoveri ricoveri)
        {
            ModelState.Remove("IdanimaleNavigation");
            // Verifica se l'animale è già ricoverato
            bool isAlreadyAdmitted = _context.Ricoveris.Any(r => r.IdAnimale == ricoveri.IdAnimale && r.IsRicoveroAttivo);

            if (isAlreadyAdmitted)
            {
                TempData["Errore"] = "L'animale selezionato è già ricoverato.";
            }
            else if (ModelState.IsValid)
            {
                ricoveri.Dataregistrazionericovero = DateTime.Now;
                ricoveri.DataInizioRicovero = DateTime.Now;
                ricoveri.IsRicoveroAttivo = true;

                _context.Add(ricoveri);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Ricovero creato con successo.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Idanimale = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale", ricoveri.IdAnimale);
            return View(ricoveri);
        }




        // GET: Ricoveri/Edit/5
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

            var ricoveri = await _context.Ricoveris.FindAsync(id);
            if (ricoveri == null)
            {
                return NotFound();
            }
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "IdAnimale", "IdAnimale", ricoveri.IdAnimale);
            return View(ricoveri);
        }

        // POST: Ricoveri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRicovero,Dataregistrazionericovero,IdAnimale,DataInizioRicovero,DataFinericovero,PrezzoGiornalieroRicovero,IsRicoveroAttivo,PrezzoTotaleRicovero")] Ricoveri ricoveri)
        {
            if (id != ricoveri.IdRicovero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ricoveri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RicoveriExists(ricoveri.IdRicovero))
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
            ViewData["Idanimale"] = new SelectList(_context.Animalis, "IdAnimale", "IdAnimale", ricoveri.IdAnimale);
            return View(ricoveri);
        }

		// GET: Ricoveri/Delete/5
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

            var ricoveri = await _context.Ricoveris
                .Include(r => r.IdAnimaleNavigation)
                .FirstOrDefaultAsync(m => m.IdRicovero == id);
            if (ricoveri == null)
            {
                return NotFound();
            }

			return View(ricoveri);
		}

		// POST: Ricoveri/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var ricoveri = await _context.Ricoveris.FindAsync(id);
			if (ricoveri != null)
			{
				_context.Ricoveris.Remove(ricoveri);
			}

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Ricoveri/Dismetti/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dismetti(int id)
        {
            var ricovero = await _context.Ricoveris.FindAsync(id);
            if (ricovero == null)
            {
                TempData["Errore"] = "Ricovero non trovato.";
                return RedirectToAction(nameof(Index));
            }

            if (ricovero.IsRicoveroAttivo)
            {
                ricovero.IsRicoveroAttivo = false;
                ricovero.DataFinericovero = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Message"] = "L'animale è stato dismesso con successo.";
            }
            else
            {
                TempData["Errore"] = "L'animale non è attualmente ricoverato.";
            }

            return RedirectToAction(nameof(Index));
        }

		private bool RicoveriExists(int id)
		{
			return _context.Ricoveris.Any(e => e.IdRicovero == id);
		}
	}
}
