using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using App_FDark.Services.concretServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Index(string Order, string DataTypeSelected, int StatusSelected, int ExtSelected, int ContentSelected)
        {
            
            //Check parameters
            if (!String.IsNullOrEmpty(DataTypeSelected))
            { 
                if (!DataTypeDictionary.dataTypeDictionary.ContainsValue(DataTypeSelected))
                {
                    return Redirect("Home/index");
                }
            }
            if (StatusSelected != 0)
            {
                if (!StatusDictionary.statusDictionary.ContainsKey(StatusSelected))
                {
                    return Redirect("Home/index");
                }
            }
            if (ExtSelected != 0)
            {
                try
                {
                    _context.Extension.Find(ExtSelected);
                }
                catch (Exception ex)
                {
                    return Redirect("Home/index");
                }
            }
            if (ContentSelected != 0)
            {
                try
                {
                    _context.Content.Find(ContentSelected);
                }
                catch (Exception ex)
                {
                    return Redirect("Home/index");
                }
            }
            //Donnée vue
            //Find Resources
            _LinksIndexViewModel vm = new _LinksIndexViewModel(StatusSelected, DataTypeSelected, ExtSelected, _context.Extension.ToList(), Order, ContentSelected);
            vm.Resources = _resourcesServices.CreateResourceAdminViewModel(Order, DataTypeSelected, StatusSelected, ExtSelected, ContentSelected);
            if (String.IsNullOrEmpty(vm.Order))
            {
                vm.Order = "id";
            }
            return View(vm);
        }

        // GET: Links/Create
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create(bool byUser)
        {
            //Donnée Vue
            _LinksCreateViewModel vm = new _LinksCreateViewModel(_context.Extension.ToList());
            if (byUser)
            {
                return View("CreateByUser",vm);
            }
            return View(vm);
        }

        // POST: Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,Url,Description,ContentId,DataType,Status,Picture")]Links newLink,bool byUser)
        {
            //Check parametres
            if (!DataTypeDictionary.dataTypeDictionary.ContainsValue(newLink.DataType))
            {
                ModelState.AddModelError("NewLink.DataType", "Type de ressources incorect");
            }
            if (byUser)
            {
                newLink.Status = 1;
            }
            else
            {
                newLink.Status = 3;
            }
       
            try
            {
                var content = _context.Content.Find(newLink.ContentId);
                if (content == null || string.IsNullOrEmpty(content.Name))
                {
                    ModelState.AddModelError("NewLink.ContentId", "Contenu introuvable");
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception, loguer, etc., si nécessaire.
            }

            if (ModelState.IsValid)
            {
                _context.Add(newLink);
                await _context.SaveChangesAsync();
                if(byUser)
                {
                    return RedirectToAction("LinksCatalog", "Home", new { newLink.ContentId });
                }
                return RedirectToAction(nameof(Index));
            }
            int extId = 0;
            if (newLink.ContentId != 0)
            {
                extId = _context.Content.Where(c => c.Id == newLink.ContentId).FirstOrDefault().ExtensionId;
            }

            //Donnée Vue
            _LinksCreateViewModel vm = new _LinksCreateViewModel(_context.Extension.ToList());
            vm.NewLink = new Links(1, newLink.Label, newLink.Url, newLink.Description, newLink.ContentId, 1, newLink.DataType);
            vm.ContentID = newLink.ContentId;
            if(byUser)
            {
                return View("CreateByUser",vm);
            }
            return View(vm);
        }

        // GET: Links/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id,int redirId)
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
            
            //Donnée Vue
            ViewData["ExtensionList"] = new SelectList(_context.Extension.ToList(), "Id", "Name",extId);
            ViewData["StatusList"] = new SelectList(statusList, "Key", "Value",links.Status);
            ViewBag.Redirect = redirId;
            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Label,Url,Description,ContentId,DataType,Status,Picture")]Links link,int redirId)
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
            //Donnée Vue
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
                if(redirId != 0)
                {
                    int contentId = redirId;
                    return RedirectToAction("LinksCatalog","Home",new { contentId });
                }
                return RedirectToAction(nameof(Index));
            }
            return View(link);
        }

        // GET: Links/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,User")]
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

        [Authorize(Roles = "Admin")]
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
