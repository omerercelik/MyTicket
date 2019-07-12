using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class TicketListModel
    {
        public int id { get; set; }
        public string kullaniciAdi { get; set; }
        public DateTime tarih { get; set; }
        public string tur { get; set; }
        public string sorumluAdi { get; set; }
        public string durum { get; set; }
    }
}
