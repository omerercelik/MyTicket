using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class Company_Principal_Model
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int PrincipalId { get; set; }
        public bool IsActive { get; set; }
    }
}
