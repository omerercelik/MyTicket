using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class CompanyAdminIslemleri
    {
        public ProductModel urunEkle(string productName, int userId)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new Exception("ürün adı boş olamaz!!!");
            if (userId <= 0)
                throw new Exception("CompanyAdmin Id 0 veya daha küçük olamaz");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company_Principal_Model cPrincipal = (from p in context.Principal.Where(x => x.IsActive)
                                                      join cp in context.Company_Principal.Where(x => x.IsActive) on p.Id equals cp.PrincipalId
                                                      where
                                                       p.Id == userId
                                                      select new Company_Principal_Model
                                                      {
                                                          Id = cp.Id,
                                                          CompanyId = cp.CompanyId,
                                                          PrincipalId = cp.PrincipalId,
                                                          IsActive = cp.IsActive

                                                      }).FirstOrDefault();
                if (cPrincipal == null)
                    throw new Exception("KULLANICIYA AİT ŞİRKET BULUNAMADI");
                Product product = new Product();
                product.IsActive = true;
                product.Name = productName;
                product.CompanyId = cPrincipal.CompanyId;
                context.Product.Add(product);
                context.SaveChanges();
                product = context.Product.FirstOrDefault(x => x.CompanyId == cPrincipal.CompanyId && x.Name == productName && x.IsActive == true);
                Principal_Product pp = new Principal_Product();
                pp.IsActive = true;
                pp.PrincipalId = userId;
                pp.ProductId = product.Id;
                context.Principal_Product.Add(pp);
                context.SaveChanges();
                if (product != null)
                {
                    ProductModel pmodel = new ProductModel();
                    pmodel.Id = product.Id;
                    pmodel.IsActive = product.IsActive;
                    pmodel.Name = product.Name;
                    pmodel.CompanyId = product.CompanyId;
                    return pmodel;
                }
                else
                    throw new Exception("Ürün Eklenemedi");



            }

        }
        public void UrunSil(int id)
        {
            if (id <= 0)
                throw new Exception("product Id sıfır veya daha küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Product p = context.Product.FirstOrDefault(x => x.Id == id);
                if (p != null)
                {
                    List<Principal_Product> pp = context.Principal_Product.Where(x => x.ProductId == id).ToList();
                    if (pp != null)
                        foreach (Principal_Product item in pp)
                            item.IsActive = false;
                    List<Team> t = context.Team.Where(x => x.ProductId == id).ToList();
                    if (t != null)
                        foreach (Team t_item in t)
                            t_item.IsActive = false;
                    p.IsActive = false;
                    context.SaveChanges();
                }
            }
        }
        public void yeniUye(string username, string password, int productId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("username  boş olamaz!!!");
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("password boş olamaz!!!");
            if (productId <= 0)
                throw new Exception("product Id sıfır veya daha küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal member = new Principal();
                member.IsActive = true;
                member.Password = password;
                member.Username = username;
                member.RoleId = 5;
                context.Principal.Add(member);
                context.SaveChanges();
                member = context.Principal.FirstOrDefault(x => x.Username == username && x.Password == password && x.IsActive && x.RoleId == 5);
                Product p = context.Product.FirstOrDefault(x => x.Id == productId && x.IsActive);
                if (p == null)
                    throw new Exception("ürün bulunamadı");
                Principal_Product pp = new Principal_Product();
                pp.IsActive = true;
                pp.PrincipalId = member.Id;
                pp.ProductId = productId;
                context.Principal_Product.Add(pp);
                Company_Principal cp = new Company_Principal();
                cp.CompanyId = p.CompanyId;
                cp.PrincipalId = member.Id;
                cp.IsActive = true;
                context.Company_Principal.Add(cp);
                Team t = new Team();
                t.Idleader = false;
                t.IsActive = true;
                t.PrincipalId = member.Id;
                t.ProductId = productId;
                context.Team.Add(t);
                context.SaveChanges();
            }
        }
        public List<CompanyAdminMemberListModel> uyeleriGetir(int productId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<CompanyAdminMemberListModel> mlist = (from p in ctx.Principal.Where(x => x.IsActive)
                                                           join t in ctx.Team.Where(x => x.IsActive) on p.Id equals t.PrincipalId
                                                           where
                                                            t.ProductId == productId
                                                           select new CompanyAdminMemberListModel
                                                           {
                                                               Id = p.Id,
                                                               username = p.Username,
                                                               ısLeader = t.Idleader
                                                           }).ToList();
                return mlist;
            }
        }
        public MevcutUyeListModel mevcutUyeleriGetir(int companyId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<mevcutUyeModel> plist = (from c in ctx.Company.Where(x => x.IsActive)
                                              join cp in ctx.Company_Principal.Where(x => x.IsActive) on c.Id equals cp.CompanyId
                                              join p in ctx.Principal.Where(x => x.IsActive) on cp.PrincipalId equals p.Id
                                              join r in ctx.Role.Where(x => x.IsActive) on p.RoleId equals r.Id
                                              where
                                               c.Id == companyId && r.Id == 5
                                              select new mevcutUyeModel
                                              {
                                                  Id = p.Id,
                                                  username = p.Username
                                              }).ToList();
                MevcutUyeListModel mList = new MevcutUyeListModel();
                mList.uyeler = plist;
                return mList;
            }
        }
        public void mevcutUyeEkle(int userId, int productId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal_Product pp = ctx.Principal_Product.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == productId && x.IsActive);
                if (pp != null)
                    throw new Exception("Üye zaten Ekli!!!");
                Team t = ctx.Team.FirstOrDefault(x => x.ProductId == productId && x.PrincipalId == userId && x.IsActive);
                if (t != null)
                    throw new Exception("üye zaten ekli");
                Principal_Product prp = new Principal_Product();
                prp.IsActive = true;
                prp.PrincipalId = userId;
                prp.ProductId = productId;
                ctx.Principal_Product.Add(prp);
                Team tt = new Team();
                tt.IsActive = true;
                tt.Idleader = false;
                tt.PrincipalId = userId;
                tt.ProductId = productId;
                ctx.Team.Add(tt);
                ctx.SaveChanges();
            }
        }
        public void uyeCıkar(int userId, int productId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal_Product pp = ctx.Principal_Product.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == productId && x.IsActive);
                if (pp == null)
                    throw new Exception("hata! üye bulunamadı");
                pp.IsActive = false;
                Team t = ctx.Team.FirstOrDefault(x => x.PrincipalId == userId && x.ProductId == productId && x.IsActive);
                if (t == null)
                    throw new Exception("hata!! üye bulunamadı");
                t.IsActive = false;
                ctx.SaveChanges();
            }
        }
        public void sorumluAta(int userId, int productId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<Team> t = ctx.Team.Where(x => x.ProductId == productId && x.IsActive).ToList();
                foreach (Team team_item in t)
                {
                    if (team_item.PrincipalId == userId)
                    {
                        team_item.Idleader = true;
                        continue;
                    }
                    team_item.Idleader = false;
                }
                ctx.SaveChanges();
            }
        }
        public int teamLeaderGetir(int productId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Team model = ctx.Team.Where(x => x.IsActive && x.Idleader == true && x.ProductId == productId).FirstOrDefault();
                int id;
                if (model == null)
                    id = 0;
                else
                    id = model.PrincipalId;

                return id;
            }
        }
    }
}
