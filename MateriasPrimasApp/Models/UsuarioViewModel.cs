using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.Models
{
    public class UsuarioViewModel : IdentityUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "El campo Contraseña y Confirmar Contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        public List<string> Roles { get; set; }
        public int UnidadOrganizativaId { get; set; }
    }
}