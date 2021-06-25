using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class IndexViewModel
    {
       public IEnumerable<SlideViewModel> slideVm;
        public IEnumerable<ProductViewModel> productHotVm;
        public IEnumerable<ProductViewModel> productPromotionVm;
        public IEnumerable<PostViewModel> postVm;
        public  string MetaKeyword;
        public  string Title;
        public  string MetaDiscription;
    }
}