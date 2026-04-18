using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.IntranetDto
{
    public class IntranetClientReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty; // "Name Surname"
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
