using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class PostCategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    
        public string Alias { get; set; }
        public int? ParentID { get; set; }
     
        public string Description { set; get; }
        public int? DisplayOrder { get; set; }

        public string Image { get; set; }
        public bool? HomeFlag { set; get; }
      
        public string MetaKeyword { get; set; }
 
        public string MetaDiscription { get; set; }
        public DateTime? CreateDate { get; set; }
   
        public string CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
      
        public string UpdatedBy { get; set; }
        public bool Status { get; set; }
        public int? HomeOrder { set; get; }

    }
}