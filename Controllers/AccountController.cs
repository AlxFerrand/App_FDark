using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App_FDark.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IResourcesServices _resourcesService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDb, IResourcesServices resourcesServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = applicationDb;
            _resourcesService = resourcesServices;
        }

        [HttpGet]
        public IActionResult Register()
        {
            //Donnée Vue
            ViewBag.CategoriesList = _context.Extension.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Username,
                    Tag = model.Tag,
                    CharacterName = model.CharacterName,
                    ServerName = model.ServerName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //Donnée Vue
            ViewBag.CategoriesList = _context.Extension.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //Model
            var vm = _context.Users.ToList();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public string Delete(string id)
        {
            if (_context.Users == null)
            {
                return "Entity set 'ApplicationDbContext.Users'  is null.";
            }
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return "ok";
            }
            return "Utilisateur inconnu";
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmeUser(string id)
        {
            ErrorViewModel error = new ErrorViewModel();
            var user = _context.Users.Find(id);
            if (user != null)
            {
                try
                {
                    var userToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, userToken);
                    await _userManager.AddToRoleAsync(user, "user");

                    //Model
                    var vm = _context.Users.ToList();
                    return View("index", vm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        //Model
                        error.RequestId = "Confirmation d'utilisateur";
                        error.MessageError = "Utilisateur inconnu";
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //Model
            error.RequestId = "Confirmation d'utilisateur";
            error.MessageError = "Utilisateur inconnu";
            return View("Error", error);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPass(string id,string newPass)
        {
            ErrorViewModel error = new ErrorViewModel();
            var user = _context.Users.Find(id);
            if (user != null)
            {
                if (String.IsNullOrEmpty(newPass))
                {
                    //Model
                    error.RequestId = "Modification de mot de passe";
                    error.MessageError = "Mot de passe vide";
                    return View("Error",error);
                }
                try
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, newPass);
                    //Model
                    var vm = _context.Users.ToList();
                    return View("index",vm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //Model
            error.RequestId = "Modification de mot de passe";
            error.MessageError = "User inconnu";
            return View("Error",error);
        }
        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id.Equals(id))).GetValueOrDefault();
        }
    }
}
