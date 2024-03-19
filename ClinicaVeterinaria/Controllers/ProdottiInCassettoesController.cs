using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
    public class ProdottiInCassettoesController : Controller
    {
        private readonly SocityPetContext _context;

        public ProdottiInCassettoesController(SocityPetContext context)
        {
            _context = context;
        }

        // GET: ProdottiInCassettoes
        public async Task<IActionResult> Index(string prodotto)
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            System.Diagnostics.Debug.WriteLine("prodotto: " + prodotto);
            if (prodotto != null)
            {
                var socityPetContext = _context.ProdottiInCassettos.Include(p => p.IdCassettoNavigation).Include(p => p.IdProdottoNavigation).Where(p => p.IdProdottoNavigation.Nomeprodotto == prodotto);
                return View(socityPetContext);    
                
            }
            else
            {
                var socityPetContext = _context.ProdottiInCassettos.Include(p => p.IdCassettoNavigation).Include(p => p.IdProdottoNavigation);
                return View(await socityPetContext.ToListAsync());
            }



            //var socityPetContext = _context.ProdottiInCassettos.Include(p => p.IdCassettoNavigation).Include(p => p.IdProdottoNavigation);

            //var query = from Cassetti in _context.Cassettis
            //            join Armadietti in _context.Armadiettis on Cassetti.IdArmadietto equals Armadietti.IdArmadietto
            //            select new { Armadietto = Armadietti.Descrizione }; // Aggiunto Armadietto.Descrizione

            //var result = query.Select(x => x.Armadietto).ToList();
            //ViewBag.ProdottiInArmadietto = result;


            //IQueryable<ProdottiInCassetto> socityPetContext = _context.ProdottiInCassetto.Include(p => p.IdCassettoNavigation).Include(p => p.IdProdottoNavigation);

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    socityPetContext = socityPetContext.Where(p => p.IdProdottoNavigation.NomeProdotto.Contains(searchString));
            //}

            //return View(await socityPetContext.ToListAsync());

            //return View(await socityPetContext.ToListAsync());
        }

        // GET: ProdottiInCassettoes/Details/5
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

            var prodottiInCassetto = await _context.ProdottiInCassettos
                .Include(p => p.IdCassettoNavigation)
                .Include(p => p.IdProdottoNavigation)
                .FirstOrDefaultAsync(m => m.IdProdottoInCassetto == id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }

            return View(prodottiInCassetto);
        }





        // GET: ProdottiInCassettoes/Create
        public IActionResult Create()
        {
            string navfoot = "farm";
            ViewBag.NavFoot = navfoot;
            string text = "wh";
            ViewBag.Text = text;

            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "Descrizione");
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "Nomeprodotto");
            return View();
        }

        // POST: ProdottiInCassettoes/Create
       






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProdottoInCassetto,IdProdotto,IdCassetto,Quantita")] ProdottiInCassetto prodottiInCassetto)
        {

            //ModelState.Remove("IdProdottoInCassetto");
            //ModelState.Remove("IdDittaFornitriceNavigation");
            if (ModelState.IsValid)
            {
                _context.Add(prodottiInCassetto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "Descrizione", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "Nomeprodotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }






        // GET: ProdottiInCassettoes/Edit/5
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

            var prodottiInCassetto = await _context.ProdottiInCassettos.FindAsync(id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "IdCassetto", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }






        // POST: ProdottiInCassettoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProdottoInCassetto,IdProdotto,IdCassetto,Quantita")] ProdottiInCassetto prodottiInCassetto)
        {
            if (id != prodottiInCassetto.IdProdottoInCassetto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodottiInCassetto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdottiInCassettoExists(prodottiInCassetto.IdProdottoInCassetto))
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
            ViewData["IdCassetto"] = new SelectList(_context.Cassettis, "IdCassetto", "IdCassetto", prodottiInCassetto.IdCassetto);
            ViewData["IdProdotto"] = new SelectList(_context.Prodottis, "IdProdotto", "IdProdotto", prodottiInCassetto.IdProdotto);
            return View(prodottiInCassetto);
        }





        // GET: ProdottiInCassettoes/Delete/5
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

            var prodottiInCassetto = await _context.ProdottiInCassettos
                .Include(p => p.IdCassettoNavigation)
                .Include(p => p.IdProdottoNavigation)
                .FirstOrDefaultAsync(m => m.IdProdottoInCassetto == id);
            if (prodottiInCassetto == null)
            {
                return NotFound();
            }

            return View(prodottiInCassetto);
        }

        // POST: ProdottiInCassettoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prodottiInCassetto = await _context.ProdottiInCassettos.FindAsync(id);
            if (prodottiInCassetto != null)
            {
                _context.ProdottiInCassettos.Remove(prodottiInCassetto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdottiInCassettoExists(int id)
        {
            return _context.ProdottiInCassettos.Any(e => e.IdProdottoInCassetto == id);
        }
    }
}
