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
    public class ContentTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesService;

        public ContentTypesController(ApplicationDbContext context, IResourcesServices resourcesServices)
        {
            _context = context;
            _resourcesService = resourcesServices;
        }

        // GET: ContentTypes
        public async Task<IActionResult> Index()
        {
            return _context.Types != null ? 
                          View(await _context.Types.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Types'  is null.");
        }

        // GET: ContentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var contentType = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentType == null)
            {
                return NotFound();
            }

            return View(contentType);
        }

        // GET: ContentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ContentType contentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contentType);
        }

        // GET: ContentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var contentType = await _context.Types.FindAsync(id);
            if (contentType == null)
            {
                return NotFound();
            }
            return View(contentType);
        }

        // POST: ContentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ContentType contentType)
        {
            if (id != contentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentTypeExists(contentType.Id))
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
            return View(contentType);
        }

        // GET: ContentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var contentType = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentType == null)
            {
                return NotFound();
            }

            return View(contentType);
        }

        // POST: ContentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Types'  is null.");
            }
            var contentType = await _context.Types.FindAsync(id);
            if (contentType != null)
            {
                _context.Types.Remove(contentType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentTypeExists(int id)
        {
          return (_context.Types?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
