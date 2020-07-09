using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OZO.Models;

namespace OZO.Controllers
{
    public class NatječajiController : Controller
    {
        private readonly PI09Context _context;

        public NatječajiController(PI09Context context)
        {
            _context = context;
        }

        // GET: Natječaji
        public async Task<IActionResult> Index()
        {
            var pI09Context = _context.Natječaji.Include(n => n.IdReferentniTipNavigation);
            return View(await pI09Context.ToListAsync());
        }

        // GET: Natječaji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natječaji = await _context.Natječaji
                .Include(n => n.IdReferentniTipNavigation)
                .FirstOrDefaultAsync(m => m.IdNatječaji == id);
            if (natječaji == null)
            {
                return NotFound();
            }

            return View(natječaji);
        }

        // GET: Natječaji/Create
        public IActionResult Create()
        {
            ViewData["IdReferentniTip"] = new SelectList(_context.ReferentniTip, "IdReferentniTip", "IdReferentniTip");
            return View();
        }

        // POST: Natječaji/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNatječaji,Naziv,Opis,Cijena,IdReferentniTip,VremenskiRok")] Natječaji natječaji)
        {
            if (ModelState.IsValid)
            {
                _context.Add(natječaji);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdReferentniTip"] = new SelectList(_context.ReferentniTip, "IdReferentniTip", "IdReferentniTip", natječaji.IdReferentniTip);
            return View(natječaji);
        }

        // GET: Natječaji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natječaji = await _context.Natječaji.FindAsync(id);
            if (natječaji == null)
            {
                return NotFound();
            }
            ViewData["IdReferentniTip"] = new SelectList(_context.ReferentniTip, "IdReferentniTip", "IdReferentniTip", natječaji.IdReferentniTip);
            return View(natječaji);
        }

        // POST: Natječaji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNatječaji,Naziv,Opis,Cijena,IdReferentniTip,VremenskiRok")] Natječaji natječaji)
        {
            if (id != natječaji.IdNatječaji)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(natječaji);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NatječajiExists(natječaji.IdNatječaji))
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
            ViewData["IdReferentniTip"] = new SelectList(_context.ReferentniTip, "IdReferentniTip", "IdReferentniTip", natječaji.IdReferentniTip);
            return View(natječaji);
        }

        // GET: Natječaji/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var natječaji = await _context.Natječaji
                .Include(n => n.IdReferentniTipNavigation)
                .FirstOrDefaultAsync(m => m.IdNatječaji == id);
            if (natječaji == null)
            {
                return NotFound();
            }

            return View(natječaji);
        }

        // POST: Natječaji/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var natječaji = await _context.Natječaji.FindAsync(id);
            _context.Natječaji.Remove(natječaji);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NatječajiExists(int id)
        {
            return _context.Natječaji.Any(e => e.IdNatječaji == id);
        }
    }
}
