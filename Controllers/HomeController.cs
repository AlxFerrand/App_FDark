using App_FDark.Data;
using App_FDark.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            ViewBag.CategoriesList = new List<Categories>(_context.Categories);
            return View();
        }

        public IActionResult LinksCatalog(int catId, int subCatId)
        {
            //Test des paramètres
            try
            {
                if (catId <= 0 
                    || _context.Categories.Find(catId).Name.Equals("") 
                    || subCatId < 0 
                    || subCatId > 0 && (_context.SubCategories.Find(subCatId).Name.Equals("")))
                {
                    return View("Index");
                }
            }catch (Exception ex)
            {
                return View("Index");
            }

            //Recupération des SubCategories
            List<SubCategories> subCategoriesList = new List<SubCategories>();
            string subCategoriesOfMyCat = _context.Categories.Find(catId).SubCatIds;
            string[] subCatSplit = subCategoriesOfMyCat.Split(',');
            foreach (var sub in subCatSplit)
            {
                subCategoriesList.Add(_context.SubCategories.Find(int.Parse(sub)));
            }


            //Récupération de Links en fontion de la subCategory
            List<Links> linksList = new List<Links>();
            if (subCatId > 0)
            {
                linksList = _context.Links.Where(l=>l.CategoryId == catId).Where(l=>l.SubCatId == subCatId).ToList();
            }
            else
            {
                linksList = _context.Links.Where(l => l.CategoryId == catId).ToList();
            }

            ViewBag.ActualCatId = catId;
            ViewBag.SubCategoriesList = subCategoriesList;
            ViewBag.CategoriesList = new List<Categories>(_context.Categories);
            return View(linksList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}