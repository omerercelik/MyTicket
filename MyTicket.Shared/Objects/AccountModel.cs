using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class AccountModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
