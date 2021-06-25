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
    [RoutePrefix("api/footer")]
    public class FooterController : ApiControllerBase
    {
        IFooterService _footerService;
        public FooterController(IFooterService footerService, IErrorService errorService) : base(errorService)
        {
            this._footerService = footerService;
        }
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                Footer footerDb = _footerService.GetAll();
                FooterViewModel footerVm = Mapper.Map<FooterViewModel>(footerDb);
                return request.CreateResponse(HttpStatusCode.OK, footerVm);
            });
        }
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Creatte(HttpRequestMessage request, FooterViewModel footerVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    if (_footerService.GetAll() == null)
                    {
                        Footer footerDb = new Footer();
                        footerDb.UpdateFooter(footerVm);
                        _footerService.Add(footerDb);
                        _footerService.SaveChange();
                        return request.CreateResponse(HttpStatusCode.Created, footerVm);
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã tồn tại");
                    }
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
            });
        }
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, FooterViewModel footerVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    Footer footerDb = _footerService.GetAll();
                    footerDb.UpdateFooter(footerVm);
                    _footerService.Update(footerDb);
                    _footerService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, footerVm);
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
               
            });
            
        }
    }

}
