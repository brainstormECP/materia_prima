using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage ="El campo Contraseña es requerido")]
        [StringLength(100, ErrorMessage = "La {0} debe tener almenos {2} dígitos y un máximo de {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La Contraseña y la confirmación de contraseña no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
