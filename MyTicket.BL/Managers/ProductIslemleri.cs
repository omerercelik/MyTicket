using MyTicket.DAL;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTicket.BL.Managers
{
    public class ProductIslemleri
    {
        public List<ProductModel> GetCompanyProducts(int companyId)
        {
            using (db0c08f50393a241e58194a7d000e43d75Entities ctx = new db0c08f50393a241e58194a7d000e43d75Entities())
            {
                List<ProductModel> result = (from p in ctx.Product.Where(x => x.IsActive)
                                             where
                                             p.CompanyId == companyId
                                             select new ProductModel
                                             {
                                                 Id = p.Id,
                                                 CompanyId = p.CompanyId,
                                                 Name = p.Name,
                                                 IsActive = p.IsActive
                                             }).ToList();

                return result;
            }
        }
    }
}
