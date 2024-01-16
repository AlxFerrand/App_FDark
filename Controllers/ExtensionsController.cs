using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;

namespace App_FDark.Controllers
{
    public class ExtensionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesService;

        public ExtensionsController(ApplicationDbContext context, IResourcesServices resourcesServices)
        {
            _context = context;
            _resourcesService = resourcesServices;
        }

        // GET: Extensions
        public async Task<IActionResult> Index()
        {
            return _context.Extension != null ? 
                          View(await _context.Extension.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }

        // GET: Extensions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Extension == null)
            {
                return NotFound();
            }

            var extension = await _context.Extension
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extension == null)
            {
                return NotFound();
            }

            return View(extension);
        }

        // GET: Extensions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Extensions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type")] Extension extension)
        {
            if (ModelState.IsValid)
            {
                _context.Add(extension);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(extension);
        }

        // GET: Extensions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Extension == null)
            {
                return NotFound();
            }

            var extension = await _context.Extension.FindAsync(id);
            if (extension == null)
            {
                return NotFound();
            }
            return View(extension);
        }

        // POST: Extensions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type")] Extension extension)
        {
            if (id != extension.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extension);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtensionExists(extension.Id))
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
            return View(extension);
        }

        // GET: Extensions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Extension == null)
            {
                return NotFound();
            }

            var extension = await _context.Extension
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extension == null)
            {
                return NotFound();
            }

            return View(extension);
        }

        // POST: Extensions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Extension == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var extension = await _context.Extension.FindAsync(id);
            if (extension != null)
            {
                _context.Extension.Remove(extension);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtensionExists(int id)
        {
          return (_context.Extension?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
