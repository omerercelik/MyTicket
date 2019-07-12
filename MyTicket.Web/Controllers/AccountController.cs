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
    public class AccountController : BaseController
    {
        #region User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new Exception("Alanlar Doldurulmalıdır");
                LoginIslemleri lI = new LoginIslemleri();
                base._currentUser = lI.loginControl(username, password);
                if (base._currentUser != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json("");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Username/password incorrect");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
        public JsonResult AddUser(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
                    throw new Exception("tüm alanlar zorunludur");
                KullaniciIslemleri kIslem = new KullaniciIslemleri();
                kIslem.yarat(username, password);
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