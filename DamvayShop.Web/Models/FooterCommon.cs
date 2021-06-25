using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class FooterCommon
    {
        public FooterViewModel footerVm;
        public IEnumerable<ProductCategoryViewModel> listCategoryProduct;
        public SupportOnlineViewModel supportOnlineVm;
        public IEnumerable<TagViewModel> tagVm;
    }
}