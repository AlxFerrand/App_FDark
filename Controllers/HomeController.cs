using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ContentType = App_FDark.Models.ContentType;

namespace App_FDark.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesServices;
        private readonly ILayoutServices _layoutServices;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IResourcesServices resourcesServices, ILayoutServices layoutServices)
        {
            _logger = logger;
            _context = context;
            _resourcesServices = resourcesServices;
            _layoutServices = layoutServices;
        }

        public IActionResult Index()
        {
            //Donnée Layout
            _layoutServices.SetActualCatId(0);
            return View();
        }

        public IActionResult ContentCatalog(int extId, int contentTypeId, int contentId)
        {
            //Test des paramètres
            try
            {
                if (extId <= 0
                    || _context.Extension.Find(extId).Name.Equals("")
                    || contentTypeId < 0
                    || contentTypeId > 0 && (_context.Types.Find(contentTypeId).Name.Equals(""))
                    || contentId < 0
                    || contentId >0 && (_context.Content.Find(contentId).Name.Equals("")))
                {
                    return RedirectToAction("Index");
                }
            }catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            //Donnée Vue
            _ContentCatalogViewModel vm = new _ContentCatalogViewModel();

            //Recupération des ContentTypes 
            List<Content> contentListOfExt = _context.Content.Where(c=>c.ExtensionId == extId).ToList();
            foreach (Content content in contentListOfExt)
            {
                bool findIt = false;
                int myContentTypeId = content.ContentTypeId;
                foreach (ContentType contentType in vm.ContentTypesList)
                {
                    if ((contentType.Id == myContentTypeId)){
                       findIt= true; 
                    }
                }
                if (!findIt)
                {
                    vm.ContentTypesList.Add(_context.Types.Find(myContentTypeId));
                }
            }

            //Récupération de Contenue en fontion de l'ext et du type
            if (contentTypeId > 0)
            {
                vm.ContentsList = _context.Content.Where(c => c.ExtensionId == extId).Where(c => c.ContentTypeId == contentTypeId).ToList();
            }
            else
            {
                vm.ContentsList = _context.Content.Where(c => c.ExtensionId == extId).ToList();
            }

            //Donnée Layout
            _layoutServices.SetActualCatId(extId);
            //Donnée Vue
            vm.ContentTypeSelected = contentTypeId;
            vm.ActualCatId = extId;

            return View(vm);
        }

        public IActionResult LinksCatalog(int contentId)
        {
            //Test paramètres
            try
            {
                if (contentId <= 0
                || _context.Content.Find(contentId).Name.Equals(""))
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

            //Récupération des liens
            List<Links> linksList = _context.Links.Where(l=>l.ContentId == contentId).ToList();
            _LinksCatalogViewModel vm = _resourcesServices.CreateCatalogViewModel(linksList.Where(l=>l.Status>=2).Where(l=>l.Status<=3).ToList());

            //Passage de paramètres à la vue
            vm.ContentSelected = _context.Content.Find(contentId);
            vm.ContentExtension= _context.Extension.Find(vm.ContentSelected.ExtensionId);
            vm.ContentTypeSelected = _context.Types.Find(vm.ContentSelected.ContentTypeId);
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult HomeAdmin()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetImagesList()
        {
            List<string> vm = new List<string>();
            string path = Path.Combine("wwwroot", "img");
            if (Directory.Exists(path))
            {
                foreach(string item in Directory.GetFiles(path))
                { 
                    vm.Add(item.Substring(8));
                }
            }  
            return View("_ImagesListing",vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archived(int id)
        {
            Links l = await _context.Links.FindAsync(id);
            if (l != null)
            {
                l.Status = 4;
                try
                {
                    _context.Update(l);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction("LinksCatalog", new { l.ContentId });
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Signal(int id)
        {
            Links l = await _context.Links.FindAsync(id);
            if (l != null)
            {
                l.Status = 2;
                try
                {
                    _context.Update(l);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction("LinksCatalog",new {l.ContentId});
        }

        public async Task<IActionResult> GetPicturesModal(int linkId)
        {
            Links link = await _context.Links.FindAsync(linkId);
            if (link != null)
            {
                if (!String.IsNullOrEmpty(link.Picture))
                {
                    string[] imagesList = link.Picture.Split(",");
                    return PartialView("_ImagesZoomModalPartial",imagesList);
                }
            }
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}