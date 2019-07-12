using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class LoginIslemleri
    {
        public AccountModel loginControl(string userName, string password)
        {

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new Exception("username veya password boş olamaz");
            using (db0c08f50393a241e58194a7d000e43d75Entities context = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                AccountModel account = (from p in context.Principal.Where(x => x.IsActive)
                                        where
                                        p.Username == userName && 
                                        p.Password == password
                                        select new AccountModel
                                        {
                                            UserId = p.Id,
                                            RoleId = p.RoleId
                                        }).FirstOrDefault();

                if (account != null)
                {
                    if (account.RoleId == 2)
                    {
                        account.Companies = (from c in context.Company
                                             select new CompanyModel
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name,
                                                 IsActive = c.IsActive
                                             }).ToList();
                        account.Products = (from p in context.Product
                                            select new ProductModel
                                            {
                                                Id = p.Id,
                                                Name = p.Name,
                                                CompanyId = p.CompanyId,
                                                IsActive = p.IsActive
                                            }).ToList();
                    }
                    else
                    {
                        account.Companies = (from c in context.Company.Where(x => x.IsActive)
                                             join cp in context.Company_Principal.Where(x => x.IsActive) on c.Id equals cp.CompanyId
                                             where
                                             cp.PrincipalId == account.UserId
                                             select new CompanyModel
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name,
                                                 IsActive = c.IsActive
                                             }).ToList();
                        account.Products = (from p in context.Product.Where(x => x.IsActive)
                                            join pp in context.Principal_Product.Where(x => x.IsActive) on p.Id equals pp.ProductId
                                            where
                                            pp.PrincipalId == account.UserId
                                            select new ProductModel
                                            {
                                                Id = p.Id,
                                                Name = p.Name,
                                                CompanyId = p.CompanyId,
                                                IsActive = p.IsActive
                                            }).ToList();
                    }
                }
                // TODO Container check
                return account;




                //Principal prpl = context.Principal.FirstOrDefault(x => x.Username == userName && x.Password == password);
                //AccountModel account = new AccountModel();
                //if (prpl != null)
                //{
                //    account.UserId = prpl.Id;
                //    var rol = from prin in context.Principal
                //              join r in context.Role on prin.RoleId equals r.Id
                //              where prin.Id == prpl.Id
                //              select r.Id;
                //    if (rol.Equals(1))
                //    {
                //        var q1 = (from p in context.Product
                //                            select p).ToList();
                //        var q2 = (from c in context.Company
                //                 select c).ToList();
                //        account.Products = (from k in q1
                //                            select new ProductModel
                //                            {
                //                                CompanyId = k.CompanyId,
                //                                Id = k.Id,
                //                                IsActive = k.IsActive,
                //                                Name = k.Name
                //                            }).ToList();
                //        account.Companies = (from c in q2
                //                            select new CompanyModel
                //                            {
                //                                Id = c.Id,
                //                                Name = c.Name,
                //                                IsActive = c.IsActive
                //                            }).ToList();
                //    }
                //    else
                //    {
                //        var q = (from p in context.Principal
                //                 join cp in context.Company_Principal on p.Id equals cp.PrincipalId
                //                 join c in context.Company on cp.CompanyId equals c.Id
                //                 join pro in context.Product on c.Id equals pro.CompanyId
                //                 where
                //                 p.Id == prpl.Id
                //                 select new
                //                 {
                //                     Product = pro,
                //                     company = c
                //                 }).ToList();
                //        account.Products = (from k in q
                //                            select new ProductModel
                //                            {
                //                                CompanyId = k.Product.CompanyId,
                //                                Id = k.Product.Id,
                //                                IsActive = k.Product.IsActive,
                //                                Name = k.Product.Name
                //                            }).ToList();
                //        account.Companies = (from k in q
                //                             select new CompanyModel
                //                             {
                //                                 Id = k.company.Id,
                //                                 Name = k.company.Name,
                //                                 IsActive = k.company.IsActive
                //                             }).ToList();
                //    }

                //    return account;
                //}
                //else
                //    return account;
            }
        }
    }
}
