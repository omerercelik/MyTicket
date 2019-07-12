using MyTicket.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    class PrincipalIslemleri
    {
        public void ekle(Principal principal)
        {
            if (principal.IsActive == false)
                throw new Exception("Eklenecek kişi pasif olamaz!!!");
            if (principal.RoleId <= 0)
                throw new Exception("Eklenecek kişinişin rol id'si 0 veya daha küçük olamaz!!!");
            if (string.IsNullOrWhiteSpace(principal.Username) || string.IsNullOrWhiteSpace(principal.Password))
                throw new Exception("Eklenecek kişinin kullanıcı adı ve parolası boş olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Role r = context.Role.FirstOrDefault(x => x.Id == principal.RoleId);
                if (r == null)
                    throw new Exception("eklenecek kişinin rolü bulunamadı!!!");
                context.Principal.Add(principal);
                context.SaveChanges();
            }
        }
        public void principalSil(int id)
        {
            if (id <= 0)
                throw new Exception("id 0 veya 0'dan küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal p = ctx.Principal.FirstOrDefault(x => x.Id == id);
                if (p != null)
                {
                    List<Ticket> tk = ctx.Ticket.Where(x => x.PrincipalId == id).ToList();
                    if (tk != null)
                        foreach (Ticket t in tk)
                            ctx.Ticket.Remove(t);
                    List<Company_Principal> cp = ctx.Company_Principal.Where(x => x.PrincipalId == id).ToList();
                    if (cp != null)
                        foreach (Company_Principal c in cp)
                            ctx.Company_Principal.Remove(c);
                    List<Principal_Product> pp = ctx.Principal_Product.Where(x => x.PrincipalId == id).ToList();
                    if (pp != null)
                        foreach (Principal_Product pr in pp)
                            ctx.Principal_Product.Remove(pr);
                    ctx.Principal.Remove(p);
                    ctx.SaveChanges();
                }

            }

        }
        public Principal principalGetir(int id)
        {
            if (id <= 0)
                throw new Exception("id 0 veya 0'dan küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Principal p = ctx.Principal.FirstOrDefault(x => x.Id == id);
                if (p == null)
                    throw new Exception("Belirtilen Id'ye göre kişi bulunamadı...");
                return p;
            }
        }

    }
}
