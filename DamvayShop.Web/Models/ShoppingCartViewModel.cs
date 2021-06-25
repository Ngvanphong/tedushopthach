using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DamvayShop.Web.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int productId;
        public ProductViewModel productViewModel;
        public int Quantity;
        public SizeViewModel SizesVm;

    }
}