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
    public class CompanyController : BaseController
    {
        #region Member
        public ActionResult Member()
        {
            List<ProductModel> products = base._currentUser.Products.Where(x => x.IsActive).ToList();
            ViewBag.AllProducts = products;
            mevcutUye uye = new mevcutUye();
            CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
            uye.mevcutUyeler = cIslem.mevcutUyeleriGetir(base._currentUser.Companies.First().Id).uyeler;
            return View(uye);
        }
        public ActionResult SearchMemberAjaxHandler(jQueryDataTableParamModel param, int productId)
        {
            try
            {

                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                List<CompanyAdminMemberListModel> list = cIslem.uyeleriGetir(productId);

                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.username).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.Id).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.Id.ToString(),
                                c.username,
                                "",
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
        public JsonResult GetTeamLeader(int productId)
        {
            try
            {
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                int id = cIslem.teamLeaderGetir(productId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { userId = id}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AssignMemberAsLead(int userId, int productId)
        {
            try
            {
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                cIslem.sorumluAta(userId, productId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult RemoveMemberFromProduct(int userId, int productId)
        {
            try
            {
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                cIslem.uyeCıkar(userId, productId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AssignExistingMemberToProduct(int userId, int productId)
        {
            try
            {
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                cIslem.mevcutUyeEkle(userId, productId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult CreateMemberAndAssignToToProduct(string username, string password, int productId)
        {
            try
            {
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                cIslem.yeniUye(username, password, productId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Product
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult SearchProductAjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {

                List<ProductModel> list = base._currentUser.Products;

                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.Name).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.Id).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.Id.ToString(),
                                c.Name,
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
                CompanyAdminIslemleri cIslem = new CompanyAdminIslemleri();
                cIslem.UrunSil(productId);
                ProductModel pm = new ProductModel();
                pm = base._currentUser.Products.Where(x => x.Id == productId).FirstOrDefault();
                base._currentUser.Products.Remove(pm);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult CreateProduct(string productName)
        {
            try
            {
                CompanyAdminIslemleri cAdminIslem = new CompanyAdminIslemleri();
                ProductModel pmodel = new ProductModel();
                pmodel = cAdminIslem.urunEkle(productName, base._currentUser.UserId);
                base._currentUser.Products.Add(pmodel);
                List<ProductModel> list = base._currentUser.Products;
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Ticket
        public ActionResult Ticket()
        {
            return View();
        }
        public ActionResult SearchTicketAjaxHandler(jQueryDataTableParamModel param)
        {

            try
            {

                TicketIslemleri tIslem = new TicketIslemleri();
                List<TicketListModel> list = tIslem.bildirimGetir(base._currentUser.UserId,base._currentUser.RoleId);

                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.id).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.tarih).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 3:
                        list = list.OrderBy(x => x.tur).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 4:
                        list = list.OrderBy(x => x.durum).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.id.ToString(),
                                c.kullaniciAdi,
                                c.tarih.ToString(),
                                c.tur,
                                c.sorumluAdi,
                                c.durum,
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
        public JsonResult GetTicketDetail(int ticketId)
        {
            try
            {
                TicketIslemleri tIslem = new TicketIslemleri();
                BildirimDetayModel dmodel = new BildirimDetayModel();
                dmodel = tIslem.bildirimDetayGetir(ticketId,base._currentUser.UserId);
                dmodel.roleId = base._currentUser.RoleId;
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(dmodel,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult RejectTicket(int id)
        {
            try
            {
                TicketIslemleri tIslem = new TicketIslemleri();
                tIslem.bildirimReddet(id);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AssignTicket(int id, int typeId, int memberId)
        {
            try
            {
                TicketIslemleri tIslem = new TicketIslemleri();
                tIslem.sorumluAta(id, typeId, memberId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult TicketAcceptance(int id)
        {
            try
            {
                TicketIslemleri tIslem = new TicketIslemleri();
                tIslem.bildirimIslemeAl(id);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult TicketClose(int id)
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
        public JsonResult GetProductMember(int productId)
        {
            try
            {
                TicketIslemleri tIslem = new TicketIslemleri();
                List<PrincipalModel> p = tIslem.urunUyelerGetir(productId, base._currentUser.UserId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(p);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        #endregion

        #region Company
        public ActionResult List()
        {
            return View();
        }
        public ActionResult SearchCompanyAjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {
                RootIslemleri rIslem = new RootIslemleri();
                List<CompanyAdminListModel> list = rIslem.sirketleriGetir();

                int ixSortCol = Convert.ToInt32(Request["iSortCol_0"]);
                int ixSortCol2 = param.iSortingCols;
                string sortDir = Request["sSortDir_0"];
                if (string.IsNullOrWhiteSpace(sortDir))
                    sortDir = "asc";
                switch (ixSortCol)
                {
                    case 1:
                        list = list.OrderBy(x => x.Name).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                    case 2:
                        list = list.OrderBy(x => x.userName).ToList();
                        if (sortDir.ToLower() == "desc")
                            list.Reverse();
                        break;
                }
                list = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var disp = (from c in list
                            select new string[]
                            {
                                c.Id.ToString(),
                                c.Name,
                                c.userName,
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
        public JsonResult RemoveCompany(int companyId)
        {
            try
            {
                RootIslemleri rIslem = new RootIslemleri();
                rIslem.SirketSil(companyId);
                CompanyModel cm = new CompanyModel();
                cm = base._currentUser.Companies.Where(x => x.Id == companyId).FirstOrDefault();
                base._currentUser.Companies.Remove(cm);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AddCompany(string companyName, string adminUsername, string adminPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
                    throw new Exception("tüm alanlar zorunludur");
                RootIslemleri rt = new RootIslemleri();
                rt.companyAdminEkle(companyName, adminUsername, adminPassword);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        #endregion
    }
}