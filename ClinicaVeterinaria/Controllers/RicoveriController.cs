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

			ViewData["Idanimale"] = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale");
			return View();
		}

		// POST: Ricoveri/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Dataregistrazionericovero,IdAnimale,DataInizioRicovero,DataFinericovero,PrezzoGiornalieroRicovero,PrezzoTotaleRicovero")] Ricoveri ricoveri)
		{
			ModelState.Remove("IdanimaleNavigation");

			if (ModelState.IsValid)
			{
				ricoveri.IsRicoveroAttivo = true;
				_context.Add(ricoveri);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["Idanimale"] = new SelectList(_context.Animalis, "IdAnimale", "IdAnimale", ricoveri.IdAnimale);
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

		private bool RicoveriExists(int id)
		{
			return _context.Ricoveris.Any(e => e.IdRicovero == id);
		}
	}
}
