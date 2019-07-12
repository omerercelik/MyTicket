using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class KullaniciBildirimDetayModel
    {
        public string firma { get; set; }
        public string urun { get; set; }
        public DateTime tarih { get; set; }
        public string Durum { get; set; }
        public string bildirim { get; set; }
    }
}
