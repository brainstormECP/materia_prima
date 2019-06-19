using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            ViewBag.RoleId = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
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
            if(viewModel.RoleId == "Administrador" && _userManager.GetUsersInRoleAsync("Administrador").Result.Count == 2)
            {
                ModelState.AddModelError("RoleId", "Ya existen 2 usuarios con el rol Administrador");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { Email = viewModel.Email, UserName = viewModel.UserName, UnidadOrganizativaId = viewModel.UnidadOrganizativaId };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    if (viewModel.RoleId != null)
                    {
                        await _userManager.AddToRoleAsync(user, viewModel.RoleId);
                    }
                    return RedirectToAction("Usuarios");
                }
                AddErrors(result);
            }
            ViewBag.RoleId = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", viewModel.RoleId);
            ViewBag.UO = new SelectList(_context.Set<UnidadOrganizativa>().Select(n => new { Id = n.Id, Nombre = n.Nombre }).ToList(), "Id", "Nombre", viewModel.UnidadOrganizativaId);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            if (user.UnidadOrganizativaId != null)
            {
                UserEditViewModel viewmodel = new UserEditViewModel() { Id = user.Id, RoleId = roles.First(), UnidadOrganizativaId = user.UnidadOrganizativaId };
                ViewBag.Usuario = user.UserName;
                ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", viewmodel.RoleId);
                ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre", viewmodel.UnidadOrganizativaId);
                return View(viewmodel);
            }
            else
            {
                UserEditViewModel viewmodel = new UserEditViewModel() { Id = user.Id, RoleId = roles.First() };
                ViewBag.Usuario = user.UserName;
                ViewBag.Roles = new MultiSelectList(_roleManager.Roles.ToList(), "Name", "Name", viewmodel.RoleId);
                ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre");
                return View(viewmodel);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(string id, [Bind("Id,RoleId, UnidadOrganizativaId")] UserEditViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(viewModel.Id);
            var roles_to_remove = await _userManager.GetRolesAsync(user);

            if (ModelState.IsValid)
            {
                user.UnidadOrganizativaId = viewModel.UnidadOrganizativaId;
                if (viewModel.RoleId != null)
                {
                    //limpiar la lista de Roles del usuario para volver a crearla 
                    await _userManager.RemoveFromRolesAsync(user, roles_to_remove);
                    await _userManager.AddToRoleAsync(user, viewModel.RoleId);
                }
                TempData["exito"] = "Usuario editado correctamente";
                return RedirectToAction("Usuarios");
            }
            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", viewModel.RoleId);
            ViewBag.UnidadOrganizativaId = new SelectList(_context.UnidadesOrganizativas.ToList(), "Id", "Nombre", viewModel.UnidadOrganizativaId);

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