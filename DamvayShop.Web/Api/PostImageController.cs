using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [Authorize]
    [RoutePrefix("api/postimage")]
    public class PostImageController : ApiControllerBase
    {
        IPostImageService _postImageService;
        public PostImageController(IErrorService errorService, IPostImageService postImageService) : base(errorService)
        {
            this._postImageService = postImageService;
        }
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request, string postId)
        {
            return CreateHttpResponse(request, () =>
            {
                try
                {
                    IEnumerable<PostImage> listPostImageDb = _postImageService.getAllByPostId(postId);
                    IEnumerable<PostImageViewModel> listPostImageVm = Mapper.Map<IEnumerable<PostImageViewModel>>(listPostImageDb);
                    return request.CreateResponse(HttpStatusCode.OK, listPostImageVm);
                }
                catch
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Chưa có ảnh nào");
                }
                
            });
        }
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, PostImageViewModel postImageVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    PostImage postImageDb = new PostImage();
                    postImageDb.UpdatePostImage(postImageVm);
                    _postImageService.Add(postImageDb);
                    _postImageService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, postImageVm);
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                                
            });
        }
        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            if (ModelState.IsValid)
            {
                return CreateHttpResponse(request, () =>
                {
                    PostImage postImge = _postImageService.GetById(id);
                    DeleteElementImage(postImge.Path);
                    _postImageService.Delete(id);
                    _postImageService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, id);
                });
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
