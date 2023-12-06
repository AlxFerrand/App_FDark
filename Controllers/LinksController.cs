using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using App_FDark.Services.concretServices;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace App_FDark.Controllers
{
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesServices;

        public LinksController(ApplicationDbContext context, IResourcesServices resourcesServices)
        {
            _context = context;
            _resourcesServices = resourcesServices;
        }

        // GET: Links
        public async Task<IActionResult> Index(string sortOrder, string dataType, string status, int extId, int contentId)
        {
            
            //Check parameters
            if (!String.IsNullOrEmpty(dataType))
            { 
                if (!DataTypeDictionary.dataTypeDictionary.ContainsValue(dataType))
                {
                    return Redirect("Home/index");
                }
            }
            int statusInt = 0;
            if (!String.IsNullOrEmpty(status))
            {
                if (StatusDictionary.statusDictionary.ContainsValue(status))
                {
                    statusInt = StatusDictionary.statusDictionary.Where(kvp=>kvp.Value.Equals(status)).Select(kvp=>kvp.Key).FirstOrDefault();
                }
                else
                {
                    return Redirect("Home/index");
                }
            }
            if (extId != 0)
            {
                try
                {
                    _context.Extension.Find(extId);
                }
                catch (Exception ex)
                {
                    return Redirect("Home/index");
                }
            }
            if (contentId != 0)
            {
                try
                {
                    _context.Content.Find(contentId);
                }
                catch (Exception ex)
                {
                    return Redirect("Home/index");
                }
            }

            //Find Resources
            List<ResourceAdminViewModel> vm = _resourcesServices.CreateResourceAdminViewModel(sortOrder, dataType, statusInt, extId,contentId);

            //Return
            ViewData["dataTypeList"] = new SelectList(DataTypeDictionary.dataTypeDictionary.Values,dataType);
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(),"Id","Name",extId);
            ViewData["statusList"] = new SelectList(StatusDictionary.statusDictionary.Values,status);
            ViewData["contentSelected"] = contentId;
            ViewData["statusSelected"] = status;
            ViewData["dataTypeSelected"] = dataType;
            ViewData["extSelected"] = extId;
            ViewData["order"] = String.IsNullOrEmpty(sortOrder) ? "id" : sortOrder;
            return View(vm);
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Links == null)
            {
                return NotFound();
            }

            var links = await _context.Links
                .FirstOrDefaultAsync(m => m.Id == id);
            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name");
            ViewData["dataTypeList"] = new SelectList(DataTypeDictionary.dataTypeDictionary.Values);
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,Picture,Url,Description,ContentId,Status")] Links links)
        {
            if (ModelState.IsValid)
            {
                _context.Add(links);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(links);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Links == null)
            {
                return NotFound();
            }

            var links = await _context.Links.FindAsync(id);
            if (links == null)
            {
                return NotFound();
            }
            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Label,Picture,Url,Description,ContentId,Status")] Links links)
        {
            if (id != links.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(links);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinksExists(links.Id))
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
            return View(links);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Links == null)
            {
                return NotFound();
            }

            var links = await _context.Links
                .FirstOrDefaultAsync(m => m.Id == id);
            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Links == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Links'  is null.");
            }
            var links = await _context.Links.FindAsync(id);
            if (links != null)
            {
                _context.Links.Remove(links);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinksExists(int id)
        {
          return (_context.Links?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public string GetContentList(int catId)
        {
            List<Content> contentList = _context.Content.Where(c => c.ExtensionId == catId).ToList();
            Console.WriteLine(JsonSerializer.Serialize(contentList));
            return JsonSerializer.Serialize(contentList) ;
        }
    }
}
