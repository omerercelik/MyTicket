using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class CompanyIslemleri
    {
        public void ekle(CompanyModel company)
        {
            if (company.IsActive == false)
                throw new Exception("Eklenecek Şirket pasif olamaz");
            if (company == null)
                throw new Exception("Eklenecek Şirket Bilgisi boş olamaz!!!");
            if (string.IsNullOrWhiteSpace(company.Name))
                throw new Exception("Şirket adı boş olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company c = new Company();
                PropertyInfo[] propInfo = company.GetType().GetProperties();
                foreach (PropertyInfo item in propInfo)
                    c.GetType().GetProperty(item.Name).SetValue(c, item.GetValue(company, null), null);
                context.Company.Add(c);
                context.SaveChanges();
            }
        }
        public void companySil(int id)
        {
            if (id <= 0)
                throw new Exception("id 0 veya 0'dan küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company c = ctx.Company.FirstOrDefault(x => x.Id == id);
                if (c != null)
                {
                    ctx.Company.Remove(c);
                    ctx.SaveChanges();
                }

            }

        }
        public Company companyGetir(int id)
        {
            if (id <= 0)
                throw new Exception("id 0 veya 0'dan küçük olamaz!!!");
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                Company c = ctx.Company.FirstOrDefault(x => x.Id == id);
                if (c == null)
                    throw new Exception("Belirtilen Id'ye göre Şirket bulunamadı...");
                return c;
            }
        }

        public List<CompanyModel> GetAllCompanies()
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<CompanyModel> models = (from c in ctx.Company
                                             select new CompanyModel
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name,
                                                 IsActive = c.IsActive
                                             }).ToList();

                return models;
            }
        }

    }


}
