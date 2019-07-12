using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTicket.Web.Models
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