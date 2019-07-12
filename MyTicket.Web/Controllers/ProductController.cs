using MyTicket.BL.Managers;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyTicket.Web.Controllers
{
    public class ProductController : BaseController
    {
        public ActionResult List()
        {
            CompanyIslemleri repo = new CompanyIslemleri();
            List<CompanyModel> companies = repo.GetAllCompanies().Where(x => x.IsActive).ToList();
            ViewBag.AllCompanies = companies;

            return View();
        }
        public ActionResult SearchProductAjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {

                KullaniciIslemleri kIslem = new KullaniciIslemleri();
                List<CompanyUrunModel> list = kIslem.sirketUrunGoster(base._currentUser.UserId);
                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.companyName).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.productName).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.productId.ToString(),
                                c.companyName,
                                c.productName,
                                ""
                            });
                JsonResult result = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = list.Count,
                    iTotalDisplayRecords = list.Count,
                    aaData = disp
                }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult RemoveProduct(int productId)
        {
            try
            {
                KullaniciIslemleri sil = new KullaniciIslemleri();
                sil.urunSil(productId, base._currentUser.UserId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult GetProductList(int companyId)
        {
            try
            {
                ProductIslemleri repo = new ProductIslemleri();
                List<ProductModel> products = repo.GetCompanyProducts(companyId);
                Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(products);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult DetachProduct(int productId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AttachProduct(int productId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AddProduct(string companyName, string productName)
        {
            try
            {
                int cId = Convert.ToInt32(companyName);
                int pId = Convert.ToInt32(productName);
                 KullaniciIslemleri kIslem = new KullaniciIslemleri();
                kIslem.urunEkle(cId, pId, base._currentUser.UserId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

    }

}