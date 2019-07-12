using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class CompanyAdminMemberListModel
    {
        public int Id { get; set; }
        public string username { get; set; }
        public bool ısLeader { get; set; }
    }
}
