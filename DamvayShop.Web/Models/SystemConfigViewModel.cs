using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class SystemConfigViewModel
    {
       
        public int ID { get; set; }

  
        public string Code { get; set; }

        public string ValueString { get; set; }

        public int? ValueInt { get; set; }
    }
}