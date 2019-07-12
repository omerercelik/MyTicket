using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTicket.Web.Models
{
    public class Account
    {
        private string _userName;
        private string _password;
        public string UserName
        {
            set
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(_userName);
                    this._userName=addr.Address ;
                }
                catch
                {
                    throw new Exception("Geçersiz mail adresi ");
                }
            }
            get
            {
                return _userName;
            }

        }
        public string Password { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}