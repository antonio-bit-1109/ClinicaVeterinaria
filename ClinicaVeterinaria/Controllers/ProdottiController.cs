using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Edit(int id, [Bind("IdProdotto,Nomeprodotto,IdDittaFornitrice,IsMedicinale,PossibiliUsi")] Prodotti prodotti, IFormFile uploadedImage)
        {
            ModelState.Remove("IdDittaFornitriceNavigation");

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
    }
}
