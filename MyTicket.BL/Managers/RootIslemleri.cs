using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class RootIslemleri
    {
        public void companyAdminEkle(string sirketAdi, string adminUsername, string adminPassword)
        {
            if (string.IsNullOrWhiteSpace(sirketAdi))
                throw new Exception("Şirket adı boş olamaz!!!");
            Company company = new Company();
            Principal p = new Principal();
            company.IsActive = true;
            company.Name = sirketAdi;
            p.IsActive = true;
            p.RoleId = 4;
            p.Password = adminPassword;
            p.Username = adminUsername;
            if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
                throw new Exception("admin username ve admin password boş olamaz");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                context.Company.Add(company);
                context.Principal.Add(p);
                context.SaveChanges();
                Company_Principal cp = new Company_Principal();
                cp.IsActive = true;
                p = context.Principal.FirstOrDefault(x => x.Username == adminUsername && x.Password == adminPassword);
                company = context.Company.FirstOrDefault(x => x.Name == sirketAdi);
                cp.CompanyId = company.Id;
                cp.PrincipalId = p.Id;
                context.Company_Principal.Add(cp);
                context.SaveChanges();
            }
        }
        public List<CompanyAdminListModel> sirketleriGetir()
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<CompanyAdminListModel> clist = (from c in ctx.Company.Where(x => x.IsActive)
                                                     join cadmin in ctx.Company_Principal.Where(x => x.IsActive) on c.Id equals cadmin.CompanyId
                                                     join p in ctx.Principal.Where(x => x.IsActive) on cadmin.PrincipalId equals p.Id
                                                     where
                                                      p.RoleId == 4
                                                     select new CompanyAdminListModel
                                                     {
                                                         Id = c.Id,
                                                         Name = c.Name,
                                                         userName = p.Username
                                                     }).ToList();
                return clist;
            }
        }
        public void SirketSil(int id)
        {
            if (id <= 0)
                throw new Exception("product Id sıfır veya daha küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company c = context.Company.FirstOrDefault(x => x.Id == id);
                if (c != null)
                {
                    List<Product> p = context.Product.Where(x => x.CompanyId == id).ToList();
                    if (p != null)
                        foreach (Product item in p)
                        {
                            item.IsActive = false;
                            List<Team> team = context.Team.Where(x => x.ProductId == item.Id).ToList();
                            if (team != null)
                                foreach (Team team_item in team)
                                    team_item.IsActive = false;
                            List<Principal_Product> ppr = context.Principal_Product.Where(x => x.ProductId == item.Id).ToList();
                            if (ppr != null)
                                foreach (Principal_Product ppr_item in ppr)
                                    ppr_item.IsActive = false;
                        }

                    List<PrincipalModel> pmodel = (from company in context.Company.Where(x => x.Id == id)
                                                   join cprincipal in context.Company_Principal on c.Id equals cprincipal.CompanyId
                                                   join princ in context.Principal.Where(x => x.IsActive) on cprincipal.PrincipalId equals princ.Id
                                                   where
                                                    princ.RoleId == 4 || princ.RoleId == 5
                                                   select new PrincipalModel
                                                   {
                                                       Id = princ.Id,
                                                       IsActive = princ.IsActive,
                                                       Username = princ.Username,
                                                       Password = princ.Password,
                                                       RoleId = princ.RoleId
                                                   }).ToList();
                    Principal principal = new Principal();
                    if (pmodel != null)
                        foreach (PrincipalModel modelitem in pmodel)
                        {
                            principal = context.Principal.FirstOrDefault(x => x.Id == modelitem.Id);
                            principal.IsActive = false;
                        }
                    c.IsActive = false;
                    context.SaveChanges();
                }
            }
        }
    }
}
