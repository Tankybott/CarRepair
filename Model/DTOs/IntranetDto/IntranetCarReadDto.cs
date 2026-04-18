using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.IntranetDto
{
    public class IntranetCarReadDto
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string OwnerFullName { get; set; } = string.Empty;
    }
}
