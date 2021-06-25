using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    public class CategoryHeaderViewModel
    {
        public IEnumerable<ProductCategoryViewModel> productCategoryVm;

        public IEnumerable<PostCategoryViewModel> postCategoryVm;

    }
}