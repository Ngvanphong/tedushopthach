using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DamvayShop.Model.Models;

namespace DamvayShop.Web.Models
{
    public class FunctionViewModel
    {

        public string ID { set; get; }

        public string Name { set; get; }

        public string URL { set; get; }

        public int DisplayOrder { set; get; }

        public string ParentId { set; get; }

        public virtual Function Parent { set; get; }

        public bool Status { set; get; }

        public string IconCss { get; set; }


        public ICollection<FunctionViewModel> ChildFunctions { set; get; }


    }
}