using AutoMapper;
using System.Collections.Generic;
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
    [RoutePrefix("api/postcategory")]
    [Authorize]
    public class PostCategoryController : ApiControllerBase
    {
        private IPostCategoryService _postCategorySevice;

        public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService) : base(errorService)
        {
            this._postCategorySevice = postCategoryService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listCategory = _postCategorySevice.GetAll();
                var listCategoryVm = Mapper.Map<IEnumerable<PostCategoryViewModel>>(listCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listCategoryVm);

                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetDetail(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                PostCategory postCategoryDb = _postCategorySevice.GetDetail(id);
                PostCategoryViewModel postCategoryVm = Mapper.Map<PostCategoryViewModel>(postCategoryDb);
                return request.CreateResponse(HttpStatusCode.OK, postCategoryVm);
            });
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    PostCategory newpostCategory = new PostCategory();
                    newpostCategory.UpdatePostCategory(postCategoryVm);
                    var category = _postCategorySevice.Add(newpostCategory);
                    _postCategorySevice.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created, postCategoryVm);
                }
                else
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    var postCategoryDb = _postCategorySevice.GetByID(postCategoryVm.ID);
                    postCategoryDb.UpdatePostCategory(postCategoryVm);
                    _postCategorySevice.Update(postCategoryDb);
                    _postCategorySevice.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created,postCategoryVm);
                }
                else
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    _postCategorySevice.Delete(id);
                    _postCategorySevice.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                return response;
            });
        }
    }
}