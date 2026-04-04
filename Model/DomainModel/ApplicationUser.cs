using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class ApplicationUser : IdentityUser
    {
        public EmployeeProfile? EmployeeProfile { get; set; }
        public ClientProfile? ClientProfile { get; set; }
    }
}
