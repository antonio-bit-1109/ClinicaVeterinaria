using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace ClinicaVeterinaria.Controllers
{
	public class CarrelloController : Controller
	{


		private readonly SocityPetContext _context;

		public CarrelloController(SocityPetContext context)
		{
			_context = context;
		}
		//
		public ActionResult Index()
		{
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            // Prendo il carrello dalla sessione
            var carrelloSession = HttpContext.Session.GetString("carrello");

			// Se il carrello esiste e non è vuoto
			if (!string.IsNullOrEmpty(carrelloSession))
			{
				// Deserializzo il carrello
				List<Carrello> carrello = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);

				// Passo il carrello alla view
				return View(carrello);
			}

			// Se il carrello è vuoto, passo una nuova lista vuota alla view
			return View(new List<Carrello>());
		}


		public IActionResult RimuoviItemDalCarrello(int id)
		{
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            var carrelloSession = HttpContext.Session.GetString("carrello");
			if (carrelloSession != null)
			{
				List<Carrello> cart = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);

				// Rimuovo tutti gli elementi con l'ID specificato
				cart.RemoveAll(item => item.Prodotto.IdProdotto == id);

				HttpContext.Session.SetString("carrello", JsonConvert.SerializeObject(cart));
				TempData["message"] = "Articolo rimosso dal carrello";
				return RedirectToAction("Index", "Carrello");
			}
			TempData["error"] = "Articolo non trovato";
			return RedirectToAction("Index", "Carrello");
		}


		public IActionResult CreaOrdine()
		{
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            //ViewData["IdUtente"] = new SelectList(_context.Utenti, "IdUtente", "Nome", ordine.IdUtente);

            // sessione di pagamento con Stripe 
            var altroNomeCarrello = HttpContext.Session.GetString("carrello");

			if (altroNomeCarrello != null)
			{
				List<Carrello> cart = JsonConvert.DeserializeObject<List<Carrello>>(altroNomeCarrello);

				var domain = "https://localhost:7058/";

				var Options = new SessionCreateOptions
				{
					SuccessUrl = domain + "Home/Index",
					CancelUrl = domain + "Home/Index",
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
					CustomerEmail = "EmailProva@gmail.it"
				};

				foreach (var item in cart)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)item.Prodotto.Prezzo * 100,
							Currency = "eur",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Prodotto.Nomeprodotto,
								Description = item.Prodotto.PossibiliUsi,
								//Images = new List<string> { domain + "images/prodotti/" + item.Prodotto.FotoProdotto }
							}
						},
						Quantity = item.Quantita
					};

					Options.LineItems.Add(sessionLineItem);
				}
				// creo la sessione di Stripe e la invio al client
				var service = new SessionService();
				Session session = service.Create(Options);
				Response.Headers.Add("Location", session.Url);
			}

			// svuoto il carrello
			HttpContext.Session.Remove("carrello");

			// reindirizzo alla pagina di successo
			return new StatusCodeResult(303);
			//return View(ordine);

		}
	}
}
