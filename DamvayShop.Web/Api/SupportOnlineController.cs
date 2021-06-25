using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [Authorize]
    [RoutePrefix("api/contact")]
    public class SupportOnlineController : ApiControllerBase
    {
        private ISupportOnlineService _supportOnlineService;
        public SupportOnlineController(ISupportOnlineService supportOnlineService, IErrorService errorService):base(errorService)
        {
            this._supportOnlineService = supportOnlineService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                SupportOnline supportOnlineDb = _supportOnlineService.Get();
                SupportOnlineViewModel supportOnlineVm = Mapper.Map<SupportOnlineViewModel>(supportOnlineDb);
                return request.CreateResponse(HttpStatusCode.OK, supportOnlineVm);
            });
        }
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, SupportOnlineViewModel supportOnlineVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    SupportOnline supportOnlineDb = new SupportOnline();
                    supportOnlineDb.UpdateSupportOnline(supportOnlineVm);
                    _supportOnlineService.Add(supportOnlineDb);
                    _supportOnlineService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, supportOnlineVm);
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            });
        }
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, SupportOnlineViewModel supportOnlineVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    SupportOnline supportOnlineDb = _supportOnlineService.Get();
                    supportOnlineDb.UpdateSupportOnline(supportOnlineVm);
                    _supportOnlineService.Update(supportOnlineDb);
                    _supportOnlineService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, supportOnlineVm);
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            });
        }


    }
}
