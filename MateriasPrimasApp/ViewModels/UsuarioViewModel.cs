using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.ViewModels
{
    public class UsuarioViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "El campo Contraseña y Confirmar Contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Display(Name = "Unidad Organizativa")]
        public int? UnidadOrganizativaId { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Ingrese una dirección de correo válida")]
        [Required]
        public string Email { get; set; }
    }
}