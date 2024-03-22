using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        //public async Task<IActionResult> Index()
        //{
        //    string navfoot = "vet";
        //    ViewBag.NavFoot = navfoot;
        //    string text = "bl";
        //    ViewBag.Text = text;

        //    var socityPetContext = _context.Visites.Include(v => v.IdAnimaleNavigation).Include(v => v.IdRicettaNavigation);
        //    return View(await socityPetContext.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var visite = await _context.Visites
                .Include(v => v.IdAnimaleNavigation)
                .Include(v => v.Ricettemediches) // Assicurati che questa sia la proprietà di navigazione corretta
                .ToListAsync();

            return View(visite);
        }

        // GET: Visite/Details/5
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

            var visita = await _context.Visites
                .Include(v => v.IdAnimaleNavigation)
                .Include(v => v.Ricettemediches)
                .FirstOrDefaultAsync(m => m.IdVisita == id);

            if (visita == null)
            {
                return NotFound();
            }

            var viewModel = new VisitaViewModel
            {
                Visita = visita,
                Ricette = visita.Ricettemediches.ToList()
            };


            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();
                foreach (var ricetta in viewModel.Ricette)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                    SELECT p.IdProdotto, p.Nomeprodotto, p.Prezzo, p.FotoProdotto
                    FROM Prodotti p
                    INNER JOIN ProdottiRicette pr ON p.IdProdotto = pr.IdProdotto
                    WHERE pr.IdRicettaMedica = @IdRicettaMedica";

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@IdRicettaMedica";
                    parameter.Value = ricetta.IdricettaMedica;
                    command.Parameters.Add(parameter);

                    var prodotti = new List<Prodotti>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var prodotto = new Prodotti
                            {
                                IdProdotto = reader.GetInt32(reader.GetOrdinal("IdProdotto")),
                                Nomeprodotto = reader.GetString(reader.GetOrdinal("Nomeprodotto")),
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Prezzo")))
                            {
                                prodotto.Prezzo = reader.GetDecimal(reader.GetOrdinal("Prezzo"));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("FotoProdotto")))
                            {
                                prodotto.FotoProdotto = reader.GetString(reader.GetOrdinal("FotoProdotto"));
                            }

                            prodotti.Add(prodotto);
                        }
                    }

                    viewModel.ProdottiPerRicetta.Add(ricetta.IdricettaMedica, prodotti);
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            return View(viewModel);
        }



        // GET: Visite/Create
        public IActionResult Create()
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var nuovaVisita = new Visite
            {
                DataVisita = DateTime.Now,
                Ricettemediches = new List<Ricettemediche>()
            };

            ViewBag.IdAnimale = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale");
            ViewBag.IdProdotto = new SelectList(_context.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
            ViewBag.Ricetta = new Ricettemediche();

            // Passa l'oggetto nuovaVisita alla view, non un nuovo oggetto Visite
            return View(nuovaVisita);
        }
        // POST: Visite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataVisita,Anamnesi,DescrizioneCura,IdAnimale,PrezzoVisita")] Visite visita, int[] prodottiId, bool aggiungiRicetta, string descrizioneRicetta)
        {
            ModelState.Remove("descrizioneRicetta");
            ModelState.Remove("IdAnimaleNavigation");

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Crea prima la visita per ottenere un ID della visita
                        _context.Visites.Add(visita);
                        await _context.SaveChangesAsync();

                        if (aggiungiRicetta)
                        {
                            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                            // Crea la ricetta con l'ID della visita appena creata
                            var ricetta = new Ricettemediche
                            {
                                Descrizione = descrizioneRicetta,
                                IdUtente = userId,
                                IdVisita = visita.IdVisita,
                                DataPrescrizione = DateTime.Now
                            };
                            _context.Ricettemediches.Add(ricetta);
                            await _context.SaveChangesAsync();

                            // Aggiorna la visita con l'ID della ricetta
                            _context.Visites.Update(visita);
                            await _context.SaveChangesAsync();

                            if (prodottiId.Length > 0)
                            {
                                foreach (var prodottoId in prodottiId)
                                {
                                    string insertSql = @"INSERT INTO ProdottiRicette (IdProdotto, IdRicettaMedica) VALUES (@p0, @p1)";
                                    await _context.Database.ExecuteSqlRawAsync(insertSql, prodottoId, ricetta.IdricettaMedica);
                                }
                            }
                        }

                        await transaction.CommitAsync();
                        TempData["Successo"] = "La visita è stata creata con successo.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        TempData["Errore"] = "Si è verificato un errore: " + ex.Message;
                    }
                }
            }

            // Ricarica le ViewData e ViewBag in caso di errore o ModelState non valido
            ViewBag.IdAnimale = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale", visita.IdAnimale);
            ViewBag.IdProdotto = new SelectList(_context.Prodottis.Where(p => p.IsMedicinale), "IdProdotto", "Nomeprodotto");
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "Descrizione");

            return View(visita);
        }
        // GET: Visite/Edit/5
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

            var visite = await _context.Visites
            .Include(v => v.Ricettemediches)
            .FirstOrDefaultAsync(v => v.IdVisita == id);
            if (visite == null)
            {
                return NotFound();
            }
            ViewBag.IdAnimale = new SelectList(_context.Animalis, "IdAnimale", "NomeAnimale", visite.IdAnimale);
            var ricettaId = visite.Ricettemediches.FirstOrDefault()?.IdricettaMedica; // Ottiego l'ID della prima ricetta se esiste
            ViewData["IdRicetta"] = new SelectList(_context.Ricettemediches, "IdricettaMedica", "Descrizione", ricettaId);
            return View(visite);
        }

        // POST: Visite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVisita,DataVisita,Anamnesi,DescrizioneCura,IdAnimale,PrezzoVisita")] Visite visite)
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
                    return RedirectToAction(nameof(Index));
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
            }
            // Ricarica le selezioni per gli elenchi a discesa
            ViewData["IdAnimale"] = new SelectList(_context.Animalis, "Idanimale", "NomeAnimale", visite.IdAnimale);
            // Non è necessario includere la ViewData per "IdRicetta" se non viene modificata
            return View(visite);
        }

        // GET: Visite/Delete/5
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

            var visite = await _context.Visites
                .Include(v => v.IdAnimaleNavigation)
                .Include(v => v.Ricettemediches)
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
            var visita = await _context.Visites
                .Include(v => v.Ricettemediches)
                .SingleOrDefaultAsync(v => v.IdVisita == id);

            if (visita == null)
            {
                return NotFound();
            }

            if (visita.Ricettemediches != null)
            {
                // Qui rimuovi ogni ricetta singolarmente
                foreach (var ricetta in visita.Ricettemediches.ToList())
                {
                    _context.Ricettemediches.Remove(ricetta);
                }
            }

            _context.Visites.Remove(visita);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool VisiteExists(int id)
        {
            return _context.Visites.Any(e => e.IdVisita == id);
        }

        public IActionResult CronistoriaVisite(int id)
        {
            string navfoot = "vet";
            ViewBag.NavFoot = navfoot;
            string text = "bl";
            ViewBag.Text = text;

            var visiteAnimale = _context.Visites

                .Where(v => v.IdAnimale == id)
                .OrderByDescending(v => v.DataVisita)
                .ToList();

            return Json(visiteAnimale);
        }
    }
}
