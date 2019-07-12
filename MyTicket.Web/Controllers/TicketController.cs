using MyTicket.BL.Managers;
using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTicket.Web.Models;

namespace MyTicket.Web.Controllers
{
    public class TicketController : BaseController
    {
        
        public ActionResult Create()
        {
            KullaniciIslemleri kIslem = new KullaniciIslemleri();
            List<CompanyModel> companies = kIslem.sirketleriGetir(base._currentUser.UserId);
            ViewBag.AllCompanies = companies;
            return View();
        }
        public JsonResult CreateTicket( int productId, string text ,string fileInfo)
        {   // TODO : Add file data
            try
            {
                KullaniciIslemleri kIslem = new KullaniciIslemleri();
                kIslem.bildirimEkle(productId,base._currentUser.UserId ,text,fileInfo);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        public ActionResult List()
        {
            return View();
        }
        public ActionResult SearchTicketAjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {
                KullaniciIslemleri kIslem = new KullaniciIslemleri();
                List<KullaniciBildirimListModel> list = kIslem.bildirimGetir(base._currentUser.UserId);
                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.firma).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.urun).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 3:
                        list = list.OrderBy(x => x.tarih).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 4:
                        list = list.OrderBy(x => x.Durum).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.id.ToString(),
                                c.firma,
                                c.urun,
                                c.tarih.ToString(),
                                c.Durum,
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

        public JsonResult CreateDetail(int ticketId)
        {
            try
            {
                throw new Exception("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public ActionResult Detail(int ticketId)
        {
            MyTicket.Shared.Objects.KullaniciBildirimDetayModel bildirim = new MyTicket.Shared.Objects.KullaniciBildirimDetayModel();
            KullaniciIslemleri kIslem = new KullaniciIslemleri();
            bildirim = kIslem.bildirimDetayGetir(ticketId);
            Models.KullaniciBildirimDetayModel detay = new Models.KullaniciBildirimDetayModel();
            detay.bildirim = bildirim.bildirim;
            detay.Durum = bildirim.Durum;
            detay.firma = bildirim.firma;
            detay.tarih = bildirim.tarih;
            detay.urun = bildirim.urun;

            return View(detay);
        }
        public JsonResult GetProductList(int companyId)
        {
            try
            {
                KullaniciIslemleri kIslem = new KullaniciIslemleri();
                List<ProductModel> products = kIslem.urunleriGetir(base._currentUser.UserId,companyId);
                Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(products);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}