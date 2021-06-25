using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DamvayShop.Data.Reponsitories;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [Authorize]
    [RoutePrefix("api/slide")]
    public class SlideController : ApiControllerBase
    {
        private ISlideService _slideService;
        public SlideController(IErrorService errorService,ISlideService slideService):base(errorService)
        {
            this._slideService = slideService;
        }
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                IEnumerable<Slide> listSlideDb = _slideService.GetAll();
                IEnumerable<SlideViewModel> listSlideVm = Mapper.Map<IEnumerable<SlideViewModel>>(listSlideDb);
                return request.CreateResponse(HttpStatusCode.OK, listSlideVm);
            });
        }

        [HttpGet]
        [Route("detail/{id:int}")]
        public HttpResponseMessage Detail(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request, () =>
            {
                Slide slideDb = _slideService.GetById(id);
                SlideViewModel slideVm = Mapper.Map<SlideViewModel>(slideDb);
                return request.CreateResponse(HttpStatusCode.OK, slideVm);
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, SlideViewModel slideVm)
        {
            if (ModelState.IsValid)
            {
                Slide slideDb = new Slide();
                slideDb.UpdateSlide(slideVm);
                _slideService.Add(slideDb);
                _slideService.SaveChange();
                return request.CreateResponse(HttpStatusCode.Created, slideVm);
            }
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, SlideViewModel slideVm)
        {
            if (ModelState.IsValid)
            {
                Slide slideDb = _slideService.GetById(slideVm.ID);
                if (slideDb.Image != slideVm.Image&&slideDb.Image!=null)
                 DeleteElementImage(slideDb.Image);
                slideDb.UpdateSlide(slideVm);
                _slideService.Update(slideDb);
                _slideService.SaveChange();
                return request.CreateResponse(HttpStatusCode.Created, slideVm);
            }
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            if (ModelState.IsValid)
            {
               Slide slideDb = _slideService.GetById(id);               
                _slideService.Delete(id);             
                _slideService.SaveChange();
                if (slideDb.Image != null)
                {
                    DeleteElementImage(slideDb.Image);
                }              
                return request.CreateResponse(HttpStatusCode.OK, id);
            }
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }


        private void DeleteElementImage(string path)
        {
            string pathMap = HttpContext.Current.Server.MapPath(path);
            if (!string.IsNullOrEmpty(pathMap))
                File.Delete(pathMap);
        }



    }
}
