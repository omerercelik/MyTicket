using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.Shared.Objects
{
    public class BildirimDetayModel
    {
        public bool isLeader { get; set; }
        public int roleId { get; set; }
        public int productId { get; set; }
        public string urunAdi { get; set; }
        public string bildirim { get; set; }
        public string tur { get; set; }
        public string durum { get; set; }
        public string sorumlu { get; set; }
        public string dosya { get; set; }
        public DateTime tarih { get; set; }
    }
}
