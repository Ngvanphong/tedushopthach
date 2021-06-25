using System;
using System.Collections.Generic;

namespace DamvayShop.Web.Models
{
    public class PostViewModel
    {
        public string ID { set; get; }

        public string Name { get; set; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public virtual PostCategoryViewModel PostCategory { get; set; }

        public int? DisplayOrder { get; set; }

        public string Description { set; get; }

        public string Content { set; get; }
        public bool? HomeFlag { get; set; }
        public int? ViewCount { get; set; }
        public string Image { get; set; }

        public virtual List<PostTagViewModel> PostTag { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDiscription { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public bool Status { get; set; }

        public string Tags { set; get; }
    }
}