using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DamvayShop.Common;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [RoutePrefix("api/tag")]
    [Authorize]
    public class TagController : ApiControllerBase
    {
        private ITagService _tagService;
        public TagController(IErrorService errorService, ITagService tagService):base(errorService)
        {
            this._tagService = tagService;
        }
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRows = 0;
                IEnumerable<Tag> listTagDb = _tagService.GetAll();
                totalRows = listTagDb.Count();
                listTagDb = listTagDb.Skip((page - 1) * pageSize).Take(pageSize);
                IEnumerable<TagViewModel> listTagVm = Mapper.Map<IEnumerable<TagViewModel>>(listTagDb);
                PaginationSet<TagViewModel> pagination = new PaginationSet<TagViewModel>
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRows,
                    Items = listTagVm
                };
                return request.CreateResponse(HttpStatusCode.OK, pagination);
            });
        }
       
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request,TagViewModel tagVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    Tag tagDb = new Tag();
                    tagDb.UpdateTag(tagVm);
                    tagDb.ID = StringHelper.ToUnsignString(tagVm.Name);
                    _tagService.Add(tagDb);
                    _tagService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, tagVm);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
               

            });
        }
        
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                _tagService.Delete(id);
                _tagService.SaveChange();
                return request.CreateResponse(HttpStatusCode.OK, id);
            });
        }
        [Route("deletealltagnotuse")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                _tagService.DeleteMultiNotUse();
                _tagService.SaveChange();
                return request.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            });
        }
    }
}
