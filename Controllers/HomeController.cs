using App_FDark.Data;
using App_FDark.Models;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
            CatalogViewModel vm = new CatalogViewModel();
            vm.Videos = new List<ResourceVideo>();
            vm.Sites = new List<ResourceSite>();
            vm.Images = new List<ResourceImage>();
            List<Links> videoList = linksList.Where(l => l.DataType == "video").ToList();
            foreach (var l in videoList)
            {
                ResourceVideo video = new ResourceVideo();
                video.Label = l.Label;
                video.Url = l.Url;
                video.Description = l.Description;
                video.VideoId = l.Url.Substring(l.Url.LastIndexOf("v=")+2 , l.Url.LastIndexOf("&") - l.Url.LastIndexOf("v=")-2);
                vm.Videos.Add(video);
            }
            List<Links> siteList = linksList.Where(l => l.DataType == "site").ToList();
            foreach (var l in siteList)
            {
                ResourceSite site = new ResourceSite();
                site.Label = l.Label;
                site.Url = l.Url;
                site.Picture = l.Picture;
                site.Description = l.Description;
                vm.Sites.Add(site);
            }
            List<Links> imageList = linksList.Where(l => l.DataType == "image").ToList();
            foreach (var l in imageList)
            {
                ResourceImage image = new ResourceImage();
                image.Label = l.Label;
                image.Description = l.Description;
                if (!String.IsNullOrEmpty(l.Picture))
                {
                    image.Pictures = l.Picture.Split(',');
                }
                else
                {
                    string[] test = { "" };
                    image.Pictures = test;
                }
                vm.Images.Add(image);
            }

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