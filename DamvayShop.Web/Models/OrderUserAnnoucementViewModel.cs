using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DamvayShop.Model.Models;

namespace DamvayShop.Web.Models
{
    public class OrderUserAnnoucementViewModel
    {
       
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public bool HasRead { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual Order Order { get; set; }
    }
}