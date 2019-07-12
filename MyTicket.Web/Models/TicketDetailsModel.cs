using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTicket.Web.Models
{
    public class TicketDetailsModel
    {
        public string urunAdi { get; set; }
        public string bildirim { get; set; }
        public string tur { get; set; }
        public string durum { get; set; }
        public string sorumlu { get; set; }
        public string dosya { get; set; }
        public DateTime tarih { get; set; }
    }
}