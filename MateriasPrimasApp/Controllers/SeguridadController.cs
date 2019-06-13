using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var usuarios = _context.Users.Include(u => u.UnidadOrganizativa).ToList();
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
            var administradores = await _userManager.GetUsersInRoleAsync("Administrador");
            if (await _userManager.IsInRoleAsync(user, "Administrador"))
            {
                if (administradores.Any(u => u.Id != user.Id && u.LockoutEnabled != true))
                {
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
                    TempData["exito"] = "Usuario bloqueado satisfactoriamente";
                    return RedirectToAction("Usuarios");

                }
                else
                {
                    TempData["error"] = "Error al bloquear el Usuario: " + user.UserName + ". Debe existir almenos un usuario Administrador activo";
                    return RedirectToAction("Usuarios");

                }

            }
            else
            {
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
                TempData["exito"] = "Usuario bloqueado satisfactoriamente";
                return RedirectToAction("Usuarios");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarPassword(ChangePasswordViewModel vewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(vewModel.UserId);
            if (ModelState.IsValid)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, vewModel.Password);
                TempData["error"] = "Error al cambiar la contraseña al usuario: " + user.UserName;
                return RedirectToAction("Usuarios", user);
            }
            TempData["exito"] = "Contraseña cambiada satisfactoriamente para el usuario: " + user.UserName;
            return BadRequest(ModelState);
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
            TempData["exito"] = "Usuario desbloqueado satisfactoriamente";
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
            if (_context.Users.Any(u => u.UserName == viewModel.UserName))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario con este nombre");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { Email = viewModel.Email, UserName = viewModel.UserName, UnidadOrganizativaId = viewModel.UnidadOrganizativaId };
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
            if (user.UnidadOrganizativaId != null)
            {
                UserEditViewModel viewmodel = new UserEditViewModel() { Id = user.Id, Roles = await _userManager.GetRolesAsync(user) as List<string>, UnidadOrganizativaId = (int)user.UnidadOrganizativaId };
                ViewBag.Usuario = user.UserName;
                ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewmodel.Roles);
                ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre");
                return View(viewmodel);

            }
            else
            {
                UserEditViewModel viewmodel = new UserEditViewModel() { Id = user.Id, Roles = await _userManager.GetRolesAsync(user) as List<string> };
                ViewBag.Usuario = user.UserName;
                ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewmodel.Roles);
                ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre");
                return View(viewmodel);

            }

        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(string id, [Bind("Id,Roles, UnidadOrganizativaId")] UserEditViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(viewModel.Id);
            var roles_to_remove = await _userManager.GetRolesAsync(user);

            if (ModelState.IsValid)
            {
                user.UnidadOrganizativaId = viewModel.UnidadOrganizativaId;
                if (viewModel.Roles != null)
                {
                    //limpiar la lista de Roles del usuario para volver a crearla 
                    await _userManager.RemoveFromRolesAsync(user, roles_to_remove);
                    await _userManager.AddToRolesAsync(user, viewModel.Roles);
                }
                TempData["exito"] = "Usuario editado correctamente";
                return RedirectToAction("Usuarios");
            }
            ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewModel.Roles);
            ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre");

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