using MateriasPrimaApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.Models
{
    public class ApplicationUser: IdentityUser
    {
        public bool Active { get; set; }
        public int? UnidadOrganizativaId { get; set; }
        public virtual UnidadOrganizativa UnidadOrganizativa { get; set; }
    }
}
