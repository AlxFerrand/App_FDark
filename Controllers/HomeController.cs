using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mime;
using ContentType = App_FDark.Models.ContentType;

namespace App_FDark.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesServices;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IResourcesServices resourcesServices)
        {
            _logger = logger;
            _context = context;
            _resourcesServices = resourcesServices;
        }

        public IActionResult Index()
        {
            ViewBag.CategoriesList = new List<Extension>(_context.Extension);
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
                    return View("Index");
                }
            }catch (Exception ex)
            {
                return View("Index");
            }
            
            //Recupération des ContentType 
            List<ContentType> contentTypeList = new List<ContentType>();

            List<Content> contentListOfExt = _context.Content.Where(c=>c.ExtensionId == extId).ToList();
            foreach (Content content in contentListOfExt)
            {
                bool findIt = false;
                int myContentTypeId = content.ContentTypeId;
                foreach (ContentType contentType in contentTypeList)
                {
                    if ((contentType.Id == myContentTypeId)){
                       findIt= true; 
                    }
                }
                if (!findIt)
                {
                    contentTypeList.Add(_context.Types.Find(myContentTypeId));
                }
            }

            //Récupération de Contenue en fontion de l'ext et du type
            List<Content> ContentListOfSelection = new List<Content>();
            if (contentTypeId > 0)
            {
                ContentListOfSelection = _context.Content.Where(c => c.ExtensionId == extId).Where(c => c.ContentTypeId == contentTypeId).ToList();
            }
            else
            {
                ContentListOfSelection = _context.Content.Where(c => c.ExtensionId == extId).ToList();
            }

            ViewBag.ActualCatId = extId;
            ViewBag.ContentTypeList = contentTypeList;
            ViewBag.ContentTypeSelect = contentTypeId;
            ViewBag.CategoriesList = new List<Extension>(_context.Extension.ToList());

            return View(ContentListOfSelection);
        }

        public IActionResult LinksCatalog(int contentId)
        {
            //Test paramètres
            try
            {
                if (contentId <= 0
                || _context.Content.Find(contentId).Name.Equals(""))
                {
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }

            //Récupération des liens
            List<Links> linksList = _context.Links.Where(l=>l.ContentId == contentId).ToList();
            CatalogViewModel vm = _resourcesServices.CreateCatalogViewModel(linksList);

            //Passage de paramètres à la vue
            Content contentSelected = _context.Content.Find(contentId);
            ViewBag.ContentExtension = _context.Extension.Find(contentSelected.ExtensionId);
            ViewBag.ContentTypeSelected = _context.Types.Find(contentSelected.ContentTypeId);
            ViewBag.ContentSelected = contentSelected;
            ViewBag.CategoriesList = new List<Extension>(_context.Extension.ToList());
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}