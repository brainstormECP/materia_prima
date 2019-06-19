using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        [Display(Name="Unidad Organizativa")]
        public int? UnidadOrganizativaId { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public string RoleId { get; set; }
    }
}
