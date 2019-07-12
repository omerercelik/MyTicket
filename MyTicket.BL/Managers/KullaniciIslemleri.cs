using MyTicket.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTicket.Shared.Objects;

namespace MyTicket.BL.Managers
{
    public class KullaniciIslemleri
    {
        public void yarat(string username, string passsword)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("kullanıcı adı boş olamaz!!!");
            if (string.IsNullOrWhiteSpace(passsword))
                throw new Exception("parola boş olamaz.");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal p = new Principal();
                p.IsActive = true;
                p.Username = username;
                p.Password = passsword;
                p.RoleId = 6;
                context.Principal.Add(p);
                context.SaveChanges();
            }
        }
        public void urunEkle(int cId, int pId, int userId)
        {
            if (cId <= 0)
                throw new Exception("ŞİRKET Id 0 dan küçük olamaz");
            if (pId <= 0)
                throw new Exception("ürün Id 0 dan küçük olamaz");
            if (userId <= 0)
                throw new Exception("kullanıcı Id 0 dan küçük olamaz");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company c = new Company();
                Product p = new Product();
                c = context.Company.FirstOrDefault(x => x.Id == cId && x.IsActive == true);
                p = context.Product.FirstOrDefault(x => x.Id == pId && x.IsActive == true);
                Company_Principal cp = new Company_Principal();
                cp = context.Company_Principal.FirstOrDefault(x => x.PrincipalId == userId && x.CompanyId == cId && x.IsActive);
                if (cp == null)
                {
                    Company_Principal cpp = new Company_Principal();
                    cpp.CompanyId = c.Id;
                    cpp.IsActive = true;
                    cpp.PrincipalId = userId;
                    context.Company_Principal.Add(cpp);
                }
                Principal_Product pp = new Principal_Product();
                pp = context.Principal_Product.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == pId && x.IsActive);
                if (pp == null)
                {
                    Principal_Product ppp = new Principal_Product();
                    ppp.IsActive = true;
                    ppp.PrincipalId = userId;
                    ppp.ProductId = p.Id;
                    context.Principal_Product.Add(ppp);
                }
                context.SaveChanges();
            }
        }
        public void urunSil(int pId, int userId)
        {
            if (pId <= 0)
                throw new Exception("ürün Id 0 dan küçük olamaz");
            if (userId <= 0)
                throw new Exception("kullanıcı Id 0 dan küçük olamaz");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Product p = new Product();
                p = context.Product.FirstOrDefault(x => x.Id == pId && x.IsActive == true);
                Principal_Product pp = new Principal_Product();
                pp = context.Principal_Product.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == pId && x.IsActive);
                if (pp != null)
                {
                    pp.IsActive = false;
                }
                context.SaveChanges();
            }
        }
        public List<CompanyUrunModel> sirketUrunGoster(int userId)
        {

            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<CompanyUrunModel> cpList = (from p in context.Principal.Where(x => x.IsActive && x.Id == userId)
                                                 join pp in context.Principal_Product.Where(x => x.IsActive) on p.Id equals pp.PrincipalId
                                                 join pro in context.Product.Where(x => x.IsActive) on pp.ProductId equals pro.Id
                                                 join c in context.Company.Where(x => x.IsActive) on pro.CompanyId equals c.Id
                                                 select new CompanyUrunModel
                                                 {
                                                     productId = pro.Id,
                                                     companyName = c.Name,
                                                     productName = pro.Name
                                                 }).ToList();
                return cpList;
            }

        }
        public void bildirimEkle(int pId, int userId, string text, string fileInfo)
        {
            if (pId <= 0)
                throw new Exception("ürün Id 0 dan küçük olamaz");
            if (userId <= 0)
                throw new Exception("kullanıcı Id 0 dan küçük olamaz");
            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("bildirim boş olamaz ");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Ticket t = new Ticket();
                t.CreateDate = DateTime.Now;
                t.Details = text;
                t.StateId = 1;
                t.PrincipalId = userId;
                t.ProductId = pId;
                t.FileInfo = fileInfo;
                t.AssigneeId = 16;
                t.TypeId = 3;
                context.Ticket.Add(t);
                context.SaveChanges();
            }
        }
        public List<KullaniciBildirimListModel> bildirimGetir(int userId)
        {

            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<KullaniciBildirimListModel> tList = (from t in context.Ticket.Where(x => x.PrincipalId == userId)
                                                          join p in context.Product.Where(x => x.IsActive) on t.ProductId equals p.Id
                                                          join c in context.Company.Where(x => x.IsActive) on p.CompanyId equals c.Id
                                                          join ts in context.TicketState.Where(x => x.IsActive) on t.StateId equals ts.Id
                                                          select new KullaniciBildirimListModel
                                                          {
                                                              firma = c.Name,
                                                              id = t.Id,
                                                              urun = p.Name,
                                                              Durum = ts.Name,
                                                              tarih = t.CreateDate
                                                          }).ToList();
                return tList;
            }

        }
        public KullaniciBildirimDetayModel bildirimDetayGetir(int ticketId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                KullaniciBildirimDetayModel tList = (from t in context.Ticket.Where(x => x.Id == ticketId)
                                                     join p in context.Product.Where(x => x.IsActive) on t.ProductId equals p.Id
                                                     join c in context.Company.Where(x => x.IsActive) on p.CompanyId equals c.Id
                                                     join ts in context.TicketState.Where(x => x.IsActive) on t.StateId equals ts.Id
                                                     select new KullaniciBildirimDetayModel
                                                     {
                                                         firma = c.Name,
                                                         bildirim = t.Details,
                                                         urun = p.Name,
                                                         Durum = ts.Name,
                                                         tarih = t.CreateDate
                                                     }).FirstOrDefault();
                return tList;
            }

        }
        public List<CompanyModel> sirketleriGetir(int UserId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<CompanyModel> Companies = (from c in context.Company.Where(x => x.IsActive)
                                          join cp in context.Company_Principal.Where(x => x.IsActive) on c.Id equals cp.CompanyId
                                          where cp.PrincipalId == UserId
                                          select new CompanyModel
                                          {
                                              Id = c.Id,
                                              Name = c.Name,
                                              IsActive = c.IsActive
                                          }).ToList();
                return Companies;
            }

        }
        public List<ProductModel> urunleriGetir(int UserId, int companyId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<ProductModel> Products = (from p in context.Product.Where(x => x.IsActive)
                                    join pp in context.Principal_Product.Where(x => x.IsActive) on p.Id equals pp.ProductId
                                    where
                                    pp.PrincipalId == UserId && p.CompanyId == companyId
                                    select new ProductModel
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        CompanyId = p.CompanyId,
                                        IsActive = p.IsActive
                                    }).ToList();
                return Products;
            }

        }

    }
}
