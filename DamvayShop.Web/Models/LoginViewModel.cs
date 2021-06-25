using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class LoginViewModel
    {

        public string UserName { set; get; }

        public string Password { get; set; }

        public bool RememberMe { set; get; }

        public string ReturnUrl { get; set; }
    }
}