using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClinicaVeterinaria.Controllers
{
	public class ProdottiController : Controller
	{
		private readonly SocityPetContext _context;

		public ProdottiController(SocityPetContext context)
		{
			_context = context;
		}

		// GET: Prodotti


		public async Task<IActionResult> Index(string prodotto)
		{
			string navfoot = "farm";
			ViewBag.NavFoot = navfoot;
			string text = "wh";
			ViewBag.Text = text;

			System.Diagnostics.Debug.WriteLine("prodotto: " + prodotto);
			if (prodotto != null)
			{
				var socityPetContext = _context.Prodottis.Include(p => p.IdDittaFornitriceNavigation).Where(p => p.Nomeprodotto == prodotto);
				return View(socityPetContext);
			}
			else
			{
				var socityPetContext = _context.Prodottis.Include(p => p.IdDittaFornitriceNavigation).OrderBy(p => p.Nomeprodotto);
				System.Diagnostics.Debug.WriteLine(socityPetContext);
				return View(await socityPetContext.ToListAsync());
			}

		}


        public async Task<IActionResult> GetArmadiettiWithCassetti(int? id)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            if (ModelState.IsValid) { 
                // Recupera i prodotti associati al cassetto con l'ID specificato
                var ProdottiNelCassetto = _context.ProdottiInCassettos.Where(p => p.IdCassetto == id).Select(p => p.IdProdotto); // Seleziona solo gli ID dei prodotti

            if (!ProdottiNelCassetto.Any())
            {
                    ViewBag.Message = "Prodotto non trovato";
                  
                }
           // nel contesto prodotti stampo i prodotti a seconda dell'id  CHE HO RECUPERATO e storato in prodotti nel cassetto
            var products = _context.Prodottis.Where(p => ProdottiNelCassetto.Contains(p.IdProdotto));

          System.Diagnostics.Debug.WriteLine("products: " + products);
            return View(await products.ToListAsync());
        }
            return NotFound();
        }




        //                            < ========================================================


		// GET: Prodotti/Details/5
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

			var prodotti = await _context.Prodottis
				.Include(p => p.IdDittaFornitriceNavigation)
				.FirstOrDefaultAsync(m => m.IdProdotto == id);

			if (prodotti == null)

			{
				return NotFound();
			}

			return View(prodotti);
		}

		// GET: Prodotti/Create

		public IActionResult Create()
		{
			string navfoot = "farm";
			ViewBag.NavFoot = navfoot;
			string text = "wh";
			ViewBag.Text = text;

			ViewData["IdDittaFornitrice"] = new SelectList(_context.Dittafornitrices, "IdDittaFornitrice", "IdDittaFornitrice");
			return View();
		}

		// POST: Prodotti/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Nomeprodotto,Prezzo,IdDittaFornitrice,IsMedicinale,PossibiliUsi")] Prodotti prodotti, IFormFile uploadedImage)
		{
			ModelState.Remove("IdDittaFornitriceNavigation");

			if (ModelState.IsValid)
			{
				if (uploadedImage != null && uploadedImage.Length > 0)
				{
					var fileName = Path.GetFileName(uploadedImage.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/prodotti", fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await uploadedImage.CopyToAsync(fileStream);
					}

					prodotti.FotoProdotto = fileName;
				}

				_context.Add(prodotti);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));

			}

			ViewData["IdDittaFornitrice"] = new SelectList(_context.Dittafornitrices, "IdDittaFornitrice", "IdDittaFornitrice", prodotti.IdDittaFornitrice);
			return View(prodotti);
		}

		// GET: Prodotti/Edit/5
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

			var prodotti = await _context.Prodottis.FindAsync(id);
			if (prodotti == null)
			{
				return NotFound();
			}
			ViewData["IdDittaFornitrice"] = new SelectList(_context.Dittafornitrices, "IdDittaFornitrice", "IdDittaFornitrice", prodotti.IdDittaFornitrice);
			return View(prodotti);
		}

		// POST: Prodotti/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdProdotto,Nomeprodotto,Prezzo,IdDittaFornitrice,IsMedicinale,PossibiliUsi")] Prodotti prodotti, IFormFile uploadedImage)
		{
			ModelState.Remove("IdDittaFornitriceNavigation");
			ModelState.Remove("uploadedImage");

			if (id != prodotti.IdProdotto)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				if (uploadedImage != null && uploadedImage.Length > 0)
				{
					var fileName = Path.GetFileName(uploadedImage.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/prodotti", fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await uploadedImage.CopyToAsync(fileStream);
					}

					prodotti.FotoProdotto = fileName;
				}

				try
				{
					_context.Update(prodotti);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProdottiExists(prodotti.IdProdotto))
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
			ViewData["IdDittaFornitrice"] = new SelectList(_context.Dittafornitrices, "IdDittaFornitrice", "IdDittaFornitrice", prodotti.IdDittaFornitrice);
			return View(prodotti);
		}

		// GET: Prodotti/Delete/5
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

			var prodotti = await _context.Prodottis
				.Include(p => p.IdDittaFornitriceNavigation)
				.FirstOrDefaultAsync(m => m.IdProdotto == id);
			if (prodotti == null)
			{
				return NotFound();
			}

			return View(prodotti);
		}

		// POST: Prodotti/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var prodotti = await _context.Prodottis.FindAsync(id);
			if (prodotti != null)
			{
				_context.Prodottis.Remove(prodotti);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProdottiExists(int id)
		{
			return _context.Prodottis.Any(e => e.IdProdotto == id);
		}


		public async Task<IActionResult> AggiungiAlCarrello(int? idProdotto)
		{
			bool prodottoGiaPresenteNelCarrello = false;
			var prodottoSelezionato = _context.Prodottis.Find(idProdotto);

			// se il prodotto esiste
			if (prodottoSelezionato != null)
			{
				// prendo il carrello dalla sessione 
				var carrelloSession = HttpContext.Session.GetString("carrello");

				// se il carrello esiste e non è vuoto
				if (carrelloSession != null)
				{
					// deserializzo il carrello 
					List<Carrello> cart = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);

					// controllo i prodotti presenti nel carrello e se un prodotto ha gia id uguale a quello in entrata come parametro
					// incremento la quantità di 1

					foreach (var item in cart)
					{
						if (item.Prodotto.IdProdotto == idProdotto)
						{
							item.Quantita++;
							prodottoGiaPresenteNelCarrello = true;

							// serializzo di nuovo il carrello e lo salvo in sessione
							HttpContext.Session.SetString("carrello", JsonConvert.SerializeObject(cart));
							TempData["Message"] = "Hai aggiunto una quantità in più";
							return RedirectToAction("Index", "Prodotti");
						}
					}
				}

				// se il prodotto non è presente nel carrello
				if (!prodottoGiaPresenteNelCarrello)
				{
					Carrello cart = new Carrello
					{
						Prodotto = new Prodotti
						{
							IdProdotto = prodottoSelezionato.IdProdotto,
							Nomeprodotto = prodottoSelezionato.Nomeprodotto,
							FotoProdotto = prodottoSelezionato.FotoProdotto,
							Prezzo = prodottoSelezionato.Prezzo,
							PossibiliUsi = prodottoSelezionato.PossibiliUsi

						},
						Quantita = 1
					};



					//prendiamo il carrello dalla sessione 
					var carrelloSessione = HttpContext.Session.GetString("carrello");

					List<Carrello> carrello = new List<Carrello>();

					//se il carrello esiste e non è vuoto lo deserializzo
					if (!string.IsNullOrEmpty(carrelloSessione))
					{
						carrello = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSessione);
					}

					//aggiungiamo il prodotto al carrello
					carrello.Add(cart);

					//serializziamo il carrello e lo salviamo in sessione
					HttpContext.Session.SetString("carrello", JsonConvert.SerializeObject(carrello));


					TempData["Message"] = "Hai aggiunto il prodotto al carrello";
					return RedirectToAction("Index", "Prodotti");
				}

			}

			TempData["Errore"] = "Il prodotto non esiste.Riprova";
			return RedirectToAction("Index", "Prodotti");
		}
	}
}
