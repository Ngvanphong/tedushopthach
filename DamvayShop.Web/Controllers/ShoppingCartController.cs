using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
       private IProductService _productService;
       private IProductQuantityService _productQantityService;
        public ShoppingCartController(IProductService productService, IProductQuantityService productQuantityService)
        {
            this._productService = productService;
            this._productQantityService = productQuantityService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        { 
            if (Session[Common.CommonConstant.SesstionCart] == null)
            {
                Session[Common.CommonConstant.SesstionCart] = new List<ShoppingCartViewModel>();
            }
            
            return View();
        }
        
        public JsonResult GetAll()
        {
            if (Session[Common.CommonConstant.SesstionCart] == null)
            {
                Session[Common.CommonConstant.SesstionCart] = new List<ShoppingCartViewModel>();
            }
            
            var cart = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            return Json(new
            {
                status=true,
                data=cart
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId,int sizeId)
        {
            if (Session[Common.CommonConstant.CountShopping] == null)
            {
                Session[Common.CommonConstant.CountShopping] = new int();
            }
            var countShopping =(int)Session[Common.CommonConstant.CountShopping];
            var shoppingCart = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            SizeViewModel sizeVm = new SizeViewModel();

               sizeVm = Mapper.Map<SizeViewModel>(_productQantityService.GetSizeById(sizeId));
               
                                      
            if (shoppingCart == null)
            {
                shoppingCart = new List<ShoppingCartViewModel>();
             
            };
            if (shoppingCart.Any(x => x.productId == productId&&x.SizesVm.ID==sizeId))
            {
                foreach(var item in shoppingCart)
                {

                    if (item.productId == productId&&item.SizesVm.ID==sizeId)
                    {
                        item.Quantity += 1;
                       
                    }
                }
            }
            else
            {
                Product product = _productService.GetById(productId);

                ShoppingCartViewModel cart = new ShoppingCartViewModel()
                {
                    productId = productId,
                    productViewModel = Mapper.Map<ProductViewModel>(product),
                    Quantity = 1,                  
                    SizesVm= sizeVm,
                };
                shoppingCart.Add(cart);
            }
            countShopping += 1;          
            Session[Common.CommonConstant.SesstionCart] = shoppingCart;
            Session[Common.CommonConstant.CountShopping] = countShopping;
            return Json(new
            {
                status = true
            });
        }
        [HttpPost]
        public JsonResult Update(string listCart)
        {
            var listCartVm = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(listCart);
            var listCartSession = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            foreach (var item in listCartSession)
            {
                foreach(var itemVm in listCartVm)
                {
                    if (itemVm.productId == item.productId&&item.SizesVm.Name==itemVm.SizesVm.Name)
                    {
                        item.Quantity = itemVm.Quantity;
                    }
                }

            }
            Session[Common.CommonConstant.SesstionCart] = listCartSession;
            Session[Common.CommonConstant.CountShopping] = listCartSession.Count();
            //getotalPrice;
            getTotalPrice();

            return Json(new
            {
                status = true
            });

        }

        private decimal getTotalPrice()
        {
            decimal totalPrice = 0;

            var listCartSession = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            
           if (Session[Common.CommonConstant.SesstionOrder]==null)
            {
                Session[Common.CommonConstant.SesstionOrder] = new OrderSession();
            }
            var orderSession = (OrderSession)Session[Common.CommonConstant.SesstionOrder];
            foreach (var item in listCartSession)
            {
                var salePrice = item.productViewModel.Price;
                if (item.productViewModel.PromotionPrice.HasValue)
                {
                    salePrice = (decimal)item.productViewModel.PromotionPrice;
                }
                totalPrice += item.Quantity * salePrice;               
            };
            orderSession.totalPrice = totalPrice;
            Session[Common.CommonConstant.SesstionOrder] = orderSession;
            return totalPrice;
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[Common.CommonConstant.SesstionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status=true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId, string size)
        {     
            var shoppingCart = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            if (shoppingCart != null)
            {
                shoppingCart.RemoveAll(x => x.productId == productId&&(x.SizesVm.Name==size|| x.SizesVm.Name == null));
            }
         
            Session[Common.CommonConstant.SesstionCart] = shoppingCart;
            Session[Common.CommonConstant.CountShopping] = shoppingCart.Count();
            return Json(new
            {
                status = true,            

            });

        }
       
    }
}