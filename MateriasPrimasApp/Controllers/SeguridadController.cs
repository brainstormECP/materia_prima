using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MateriasPrimasApp.Controllers
{
    public class SeguridadController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SeguridadController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Usuarios()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

       

        [HttpGet]
        public async Task<IActionResult> BloquearUsuario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            int forDays = 36500;
            if (forDays > 0)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(forDays));
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            var block = await _userManager.IsLockedOutAsync(user);
            return RedirectToAction("Usuarios");
        }

        [HttpGet]
        public async Task<IActionResult> DesbloquearUsuario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.SetLockoutEnabledAsync(user, false);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
            }            
            var block = await _userManager.IsLockedOutAsync(user);
            return RedirectToAction("Usuarios");
        }


        [HttpGet]
        public IActionResult CrearUsuario()
        {
            ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name");
            ViewBag.UO = new SelectList(_context.Set<UnidadOrganizativa>().Select(n => new { Id = n.Id, Nombre = n.Nombre }).ToList(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario(UsuarioViewModel viewModel)
        {
            if(_context.Users.Any(u=>u.UserName == viewModel.UserName))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario con este nombre");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { Email = viewModel.Email, UserName = viewModel.UserName, UnidadOrganizativaId = viewModel.UnidadOrganizativaId};
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    if (viewModel.Roles != null)
                    {
                        await _userManager.AddToRolesAsync(user, viewModel.Roles);
                    }
                    return RedirectToAction("Usuarios");
                }
                AddErrors(result);
            }
            ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewModel.Roles);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            UserEditViewModel viewmodel = new UserEditViewModel() { Id = user.Id, Roles = await _userManager.GetRolesAsync(user) as List<string> };

            ViewBag.Usuario = user.UserName;
            ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewmodel.Roles);

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(string id, [Bind("Id,Roles")] UserEditViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(viewModel.Id);
            var roles_to_remove = await _userManager.GetRolesAsync(user);

            if (ModelState.IsValid)
            {
                if(viewModel.Roles != null)
                {
                    //limpiar la lista de Roles del usuario para volver a crearla 
                    await _userManager.RemoveFromRolesAsync(user, roles_to_remove );
                    await _userManager.AddToRolesAsync(user, viewModel.Roles);
                }
                return RedirectToAction("Usuarios");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            return View(user);
        }

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(string password)
        //{
        //   await _userManager.RemovePasswordAsync()
        //    return View();
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}