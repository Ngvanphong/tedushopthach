using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;
using DamvayShop.Web.SignalR;

namespace DamvayShop.Web.Controllers
{
    public class CheckoutController : Controller
    {
        public IOrderService _orderService;
        public CheckoutController(IOrderService orderService)
        {
            this._orderService = orderService;
        }
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadDistrict(int provinceId)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/xml/Provinces_HCM.xml"));
            var xmlElement = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceId);

            List<DistrictViewModel> listDistrict = new List<DistrictViewModel>();
             foreach(var item in xmlElement.Elements("Item").Where(x=>x.Attribute("type").Value == "district"))
            {
                DistrictViewModel district = new DistrictViewModel()
                {
                    ID = int.Parse(item.Attribute("id").Value),
                    Name = item.Attribute("value").Value,
                };
                listDistrict.Add(district);
            }
            return Json(new
            {
                status=true,
                data=listDistrict,
            });
           
        }

        public JsonResult GetTaxHCM(int districtId) { 
  
            string taxTransfar="12.000";
            List<int> listOutsiteDistrict = new List<int>()
            {
                70139,70143,70135,70137,70141,70134,70133
            };
            foreach(var item in listOutsiteDistrict)
            {              
                if (item == districtId)
                {
                    taxTransfar = "14.000";
                }               
            }           
            return Json(new
            {
                status = true,
                data= taxTransfar,
            });
            
        }

        public JsonResult GotoComfirm(int totalPrice,string orderVm)
        {
            if (Session[Common.CommonConstant.SesstionOrder] == null)
            {
                Session[Common.CommonConstant.SesstionOrder] = new OrderSession();
            }
            var orderSession = (OrderSession)Session[Common.CommonConstant.SesstionOrder];
            orderSession.totalPrice += totalPrice;
            orderSession.taxTransferPrice = totalPrice;
            var orderVmJson = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderVm);
            orderSession.orderVm = orderVmJson;
            orderSession.orderVm.CreateDate = DateTime.Now;
            Session[Common.CommonConstant.SesstionOrder] = orderSession;
            return Json(new
            {
                status=true,
            });

        }
        
        public ActionResult OverViewResult()
        {
           
            var orderSession = (OrderSession)Session[Common.CommonConstant.SesstionOrder];
            decimal totalPrice = orderSession.totalPrice;
            decimal transferPrice = orderSession.taxTransferPrice;
            ViewBag.totalPrice = totalPrice;
            ViewBag.transferTax = string.Format("{0:n0}", transferPrice*1000);

            return View();
        }

        public JsonResult CreateOrder()
        {

            var orderSession = (OrderSession)Session[Common.CommonConstant.SesstionOrder];
            var cartShopping = (List<ShoppingCartViewModel>)Session[Common.CommonConstant.SesstionCart];
            Order orderDb = new Order();
            orderDb.UpdateOrder(orderSession.orderVm);
            orderDb.Status = true;
            orderDb.TotalPayment = orderSession.totalPrice;
            _orderService.Create(orderDb);
            _orderService.Save();

            //push hup
            var annoucement = _orderService.GetDetail(orderDb.ID);
            DamvayShopHub.PushToAllUsers(Mapper.Map<OrderViewModel>(annoucement), null);

            foreach (var item in cartShopping)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderID = orderDb.ID;
                decimal price = 0;
                if (item.productViewModel.PromotionPrice.HasValue)
                {
                    price = (decimal)item.productViewModel.PromotionPrice;
                }
                else
                {
                    price = item.productViewModel.Price;
                }
                orderDetail.Price = price;
                orderDetail.Quantity = item.Quantity;
                orderDetail.SizeId = item.SizesVm.ID;
                orderDetail.ProductID = item.productViewModel.ID;
                _orderService.CreateDetail(orderDetail);
                _orderService.Save();              
            }

            Session[Common.CommonConstant.SesstionCart] = new List<ShoppingCartViewModel>();
            Session[Common.CommonConstant.SesstionOrder] = new OrderSession();
            Session[Common.CommonConstant.CountShopping] = new int();

            return Json(new
            {
                status=true,
            });
        }


    }
}