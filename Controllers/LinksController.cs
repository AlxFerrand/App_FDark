using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using App_FDark.Services.concretServices;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace App_FDark.Controllers
{
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesServices;
        private readonly ISaveFilesService _saveFilesService;

        public LinksController(ApplicationDbContext context, IResourcesServices resourcesServices, ISaveFilesService saveFilesService)
        {
            _context = context;
            _resourcesServices = resourcesServices;
            _saveFilesService = saveFilesService;
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
            ViewData["contentId"] = 0;
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name");
            ViewData["dataTypeList"] = new SelectList(DataTypeDictionary.dataTypeDictionary.Values);
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,Url,Description,ContentId,DataType,Status,Picture")]Links newLink)
        {
            //Check parametres
            if (!DataTypeDictionary.dataTypeDictionary.ContainsValue(newLink.DataType))
            {
                ModelState.AddModelError("DataType", "Type de ressources incorect");
            }
            newLink.Status = 1;
            try
            {
                var content = _context.Content.Find(newLink.ContentId);
                if (content == null || string.IsNullOrEmpty(content.Name))
                {
                    ModelState.AddModelError("ContentId", "Contenu introuvable");
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception, loguer, etc., si nécessaire.
            }

            //Envoie donnée au service
            if (ModelState.IsValid)
            {
                _context.Add(newLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            int extId = 0;
            if (newLink.ContentId != 0)
            {
                extId = _context.Content.Where(c => c.Id == newLink.ContentId).FirstOrDefault().ExtensionId;
            }

            Links link = new Links(1, newLink.Label, newLink.Url, newLink.Description, newLink.ContentId, 1, newLink.DataType);
            ViewData["contentId"] = newLink.ContentId;
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name",extId);
            ViewData["dataTypeList"] = new SelectList(DataTypeDictionary.dataTypeDictionary.Values,link.DataType);
            return View(link);
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
            int extId = _context.Content.Where(c => c.Id == links.ContentId).FirstOrDefault().ExtensionId;
            List<KeyValuePair<int, string>> statusList = StatusDictionary.statusDictionary.ToList();
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name",extId);
            ViewData["StatusList"] = new SelectList(statusList, "Key", "Value",links.Status);
            ViewBag.Redirect = false;
            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Label,Url,Description,ContentId,DataType,Status,Picture")]Links link)
        {
            //Check parametres
            try
            {
                var content = _context.Content.Find(link.ContentId);
                if (content == null || string.IsNullOrEmpty(content.Name))
                {
                    ModelState.AddModelError("ContentId", "Contenu introuvable");
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception, loguer, etc., si nécessaire.
            }

            int extId = _context.Content.Where(c => c.Id == link.ContentId).FirstOrDefault().ExtensionId;
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name", extId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(link);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinksExists(link.Id))
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
            ViewBag.Redirect = false;
            return View(link);
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

        //GET: SnapCard/5
        public async Task<IActionResult> GetSnapCard(int Id,string Label, string Url, string Description,string DataType,string Picture)
        {
            var snapResource = _resourcesServices.MakeSnapResource(Id,DataType, Label, Url, Description, Picture);
            if (snapResource == null)
            {
                return null;
            }
            switch (DataType)
            {
                case "video":
                    return PartialView("_VideoSnapCard", snapResource);
                    break;
                case "site":
                    return PartialView("_SiteSnapCard", snapResource);
                    break;
                case "img":
                    return PartialView("_ImageSnapCard", snapResource);
                    break;
                case "text":
                    return null;
                    break;
                    default:
                    return null;
                    break;
            }
            
        }

        public string AddPictureFiles(string newPictureLabel)
        {
            if (string.IsNullOrEmpty(newPictureLabel))
            {
                return "Error : Nom de fichier vide";
            }
            else
            {
                List<string> picturesList = new List<string>();
                string path = Path.Combine("wwwroot", "img");
                if (Directory.Exists(path))
                {
                    foreach (string item in Directory.GetFiles(path))
                    {
                        picturesList.Add(item.Substring(12));
                    }
                }
                foreach (string item in picturesList)
                {
                    string regexLabel = Regex.Replace(newPictureLabel, "[^a-zA-Z0-9_]", "");
                    if (item.StartsWith(regexLabel + ".", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Error : Nom de fichier déjà utilisé";
                    }
                }
            }
            if (Request.Form.Files.Count() <= 0)
            {
                return "Error : pas de fichiers";
            }
            foreach (var file in Request.Form.Files)
            {
                return _saveFilesService.SaveFileToImgDirectory(file, newPictureLabel);
            }
            return "Error : interne";
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
