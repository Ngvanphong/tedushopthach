using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [Authorize]
    [RoutePrefix("api/orderuser")]
    public class OrderUserAnnoucementController : ApiControllerBase
    {
        private IOrderUserAnnoucementService _annoucementService;
 
        public OrderUserAnnoucementController(IErrorService errorService, IOrderUserAnnoucementService annoucementService):base(errorService)
        {
            this._annoucementService = annoucementService;
        }
        [Route("getTopMyAnnouncement")]
        [HttpGet]
        public HttpResponseMessage GetTopMyAnnouncement(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                IEnumerable<Order> model = _annoucementService.ListAllUnread(User.Identity.GetUserId(), 1, 10, out totalRow);
                IEnumerable<OrderViewModel> modelVm = Mapper.Map<IEnumerable<OrderViewModel>>(model);

                PaginationSet<OrderViewModel> pagedSet = new PaginationSet<OrderViewModel>()
                {
                    PageIndex = 1,
                    TotalRows = totalRow,
                    PageSize = 10,
                    Items = modelVm
                };
                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("markAsRead")]
        [HttpGet]
        public HttpResponseMessage MarkAsRead(HttpRequestMessage request, int orderId)
        {

            return CreateHttpResponse(request, () =>
            {
                OrderUserAnnoucement query = _annoucementService.GetById(orderId, User.Identity.GetUserId());
                if (query == null)
                {
                    OrderUserAnnoucement orderUser = new OrderUserAnnoucement();
                    orderUser.UserId = User.Identity.GetUserId();
                    orderUser.OrderId = orderId;
                    orderUser.HasRead = true;
                    _annoucementService.Add(orderUser);
                    _annoucementService.SaveChange();
                }
                else
                {
                    query.HasRead = true;
                    _annoucementService.SaveChange();
                }
                return request.CreateResponse(HttpStatusCode.OK, orderId);
               


            });
            
        }


    }
}
