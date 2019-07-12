using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class TicketIslemleri
    {
        public List<TicketListModel> bildirimGetir(int userId, int roleId)
        {
            if (userId <= 0)
                throw new Exception("kullanici Id 0 dan küçük olamamaz.");
            if (roleId <= 0)
                throw new Exception("rol Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<TicketListModel> tlist = new List<TicketListModel>();
                if (roleId == 4)
                {
                    tlist = (from t in ctx.Ticket
                             join b in ctx.Principal on t.PrincipalId equals b.Id
                             join tt in ctx.TicketType on t.TypeId equals tt.Id
                             join s in ctx.Principal on t.AssigneeId equals s.Id
                             join d in ctx.TicketState on t.StateId equals d.Id
                             join p in ctx.Product on t.ProductId equals p.Id
                             join cp in ctx.Company_Principal on p.CompanyId equals cp.CompanyId
                             where cp.PrincipalId == userId
                             select new TicketListModel
                             {
                                 id = t.Id,
                                 kullaniciAdi = b.Username,
                                 tarih = t.CreateDate,
                                 durum = d.Name,
                                 sorumluAdi = s.Username,
                                 tur = tt.Name
                             }).ToList();
                }
                else
                {
                    tlist = (from t in ctx.Ticket
                             join b in ctx.Principal on t.PrincipalId equals b.Id
                             join tt in ctx.TicketType on t.TypeId equals tt.Id
                             join s in ctx.Principal on t.AssigneeId equals s.Id
                             join d in ctx.TicketState on t.StateId equals d.Id
                             join p in ctx.Product on t.ProductId equals p.Id
                             join team in ctx.Team on p.Id equals team.ProductId
                             where team.PrincipalId == userId && (team.Idleader == true || t.AssigneeId == userId)
                             select new TicketListModel
                             {
                                 id = t.Id,
                                 kullaniciAdi = b.Username,
                                 tarih = t.CreateDate,
                                 durum = d.Name,
                                 sorumluAdi = s.Username,
                                 tur = tt.Name
                             }).ToList();
                }
                return tlist;
            }
        }
        public BildirimDetayModel bildirimDetayGetir(int ticketId, int userId)
        {
            if (ticketId <= 0)
                throw new Exception("bildirim Id 0 dan küçük olamamaz.");
            if (userId <= 0)
                throw new Exception("kullanici Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                BildirimDetayModel dList = (from t in context.Ticket.Where(x => x.Id == ticketId)
                                            join p in context.Product on t.ProductId equals p.Id
                                            join ts in context.TicketState on t.StateId equals ts.Id
                                            join tt in context.TicketType on t.TypeId equals tt.Id
                                            join s in context.Principal on t.AssigneeId equals s.Id
                                            select new BildirimDetayModel
                                            {
                                                urunAdi = p.Name,
                                                bildirim = t.Details,
                                                dosya = t.FileInfo,
                                                durum = ts.Name,
                                                sorumlu = s.Username,
                                                tur = tt.Name,
                                                tarih = t.CreateDate,
                                                productId = t.ProductId
                                            }).FirstOrDefault();
                Ticket ticket = context.Ticket.FirstOrDefault(x => x.Id == ticketId);
                Team team = context.Team.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == ticket.ProductId && x.IsActive);
                if (team != null)
                    dList.isLeader = team.Idleader;
                return dList;
            }

        }
        public List<PrincipalModel> urunUyelerGetir(int productId, int userId)
        {
            if (productId <= 0)
                throw new Exception("ürün Id 0 dan küçük olamamaz.");
            if (userId <= 0)
                throw new Exception("kullanici Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<PrincipalModel> member = (from t in context.Team.Where(x => x.ProductId == productId && x.IsActive)
                                               join p in context.Principal.Where(x => x.IsActive && x.Id != userId) on t.PrincipalId equals p.Id
                                               select new PrincipalModel
                                               {
                                                   Id = p.Id,
                                                   Username = p.Username
                                               }).ToList();
                return member;
            }

        }
        public void sorumluAta(int ticketId, int typeId, int memberId)
        {
            if (ticketId <= 0)
                throw new Exception("bildirim Id 0 dan küçük olamamaz.");
            if (typeId <= 0)
                throw new Exception("Tür Id 0 dan küçük olamamaz.");
            if (memberId <= 0)
                throw new Exception("üye Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Ticket t = context.Ticket.FirstOrDefault(x => x.Id == ticketId);
                t.AssigneeId = memberId;
                t.TypeId = typeId;
                context.SaveChanges();
            }
        }
        public void bildirimReddet(int ticketId)
        {
            if (ticketId <= 0)
                throw new Exception("bildirim Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Ticket t = context.Ticket.FirstOrDefault(x => x.Id == ticketId);
                t.StateId = 3;
                context.SaveChanges();
            }
        }
        public void bildirimIslemeAl(int ticketId)
        {
            if (ticketId <= 0)
                throw new Exception("bildirim Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Ticket t = context.Ticket.FirstOrDefault(x => x.Id == ticketId);
                t.StateId = 2;
                context.SaveChanges();
            }
        }
        public void bildirimKapama(int ticketId)
        {
            if (ticketId <= 0)
                throw new Exception("bildirim Id 0 dan küçük olamamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Ticket t = context.Ticket.FirstOrDefault(x => x.Id == ticketId);
                t.StateId = 4;
                context.SaveChanges();
            }
        }
    }
}
