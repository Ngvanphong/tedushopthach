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
    [RoutePrefix("api/systemconfig")]
    public class SystemConfigController : ApiControllerBase
    {
        private ISystemConfigService _systemConfigService;
        public SystemConfigController(ISystemConfigService systemConfigService, IErrorService errorService):base(errorService)
        {
            this._systemConfigService = systemConfigService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
             {
                 IEnumerable<SystemConfig> listSystemConfig = _systemConfigService.GetAll();
                 IEnumerable<SystemConfigViewModel> listSystemConfigVm = Mapper.Map<IEnumerable<SystemConfigViewModel>>(listSystemConfig);
                 return request.CreateResponse(HttpStatusCode.OK, listSystemConfigVm);
             });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Detail(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request, () =>
            {
                SystemConfig systemConfig = _systemConfigService.Detail(id);
                SystemConfigViewModel systemConfigVm = Mapper.Map<SystemConfigViewModel>(systemConfig);
                return request.CreateResponse(HttpStatusCode.OK, systemConfigVm);
            });
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, SystemConfigViewModel systemConfigVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    SystemConfig systemConfigDb = new SystemConfig();
                    systemConfigDb.UpdateSystemConfig(systemConfigVm);
                    _systemConfigService.Add(systemConfigDb);
                    _systemConfigService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, systemConfigVm);
                }
                else
                   return request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, SystemConfigViewModel systemConfigVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    SystemConfig systemConfigDb = _systemConfigService.Detail(systemConfigVm.ID);
                    systemConfigDb.UpdateSystemConfig(systemConfigVm);
                    _systemConfigService.Update(systemConfigDb);                  
                    _systemConfigService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, systemConfigVm);
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>{
                _systemConfigService.Delete(id);
                _systemConfigService.SaveChange();
                return request.CreateResponse(HttpStatusCode.OK, id);
            });
        }

    }
}
