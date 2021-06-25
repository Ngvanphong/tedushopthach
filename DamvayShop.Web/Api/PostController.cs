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
    [RoutePrefix("api/post")]
    [Authorize]
    public class PostController : ApiControllerBase
    {
        private IPostService _postService;
        private IPostImageService _postImageService;
       

        public PostController(IErrorService errorService, IPostService postService,IPostImageService postImageService) : base(errorService)
        {
            this._postService = postService;
            this._postImageService = postImageService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage get(HttpRequestMessage request, int page, int pageSize=10, string keyword="")
        {
            return CreateHttpResponse(request, () =>
            {

                int totalRows = 0;
                IEnumerable<Post> listPostDb = _postService.GetAll();
                if (!string.IsNullOrEmpty(keyword))
                {
                    listPostDb = listPostDb.Where(x => x.Name.Contains(keyword));
                }
                totalRows = listPostDb.Count();
                
                listPostDb = listPostDb.OrderByDescending(x => x.CreateDate).Skip((page - 1) * pageSize).Take(pageSize);
                IEnumerable<PostViewModel> listPostVm = Mapper.Map<List<PostViewModel>>(listPostDb);
                PaginationSet<PostViewModel> pagination = new PaginationSet<PostViewModel>()
                {
                    PageIndex = page,
                    PageSize=pageSize,
                    TotalRows=totalRows,
                    Items=listPostVm,                   
                };
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagination);
                return response;
            });
        }


        [Route("detail")]
        [HttpGet]
        public HttpResponseMessage GetDetail (HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                Post postDb = _postService.GetById(id);
                PostViewModel postVm = Mapper.Map<PostViewModel>(postDb);
                return request.CreateResponse(HttpStatusCode.OK, postVm);
            });
           

        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage post(HttpRequestMessage request, PostViewModel postVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    Post postDb = new Post();
                    postDb.UpdatePost(postVm);
                    postDb.CreateDate = DateTime.Now;
                    postDb.UpdatedDate = DateTime.Now;
                    _postService.Add(postDb);
                    _postService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created, postVm);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage put(HttpRequestMessage request, PostViewModel postVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    Post postDb = _postService.GetById(postVm.ID);
                    if (postDb.Image!=postVm.Image&&postDb.Image!=null)
                    {
                        DeleteElementImage(postDb.Image.ToString());
                    }
                    postDb.UpdatePost(postVm);                                             
                    _postService.Update(postDb);

                    _postService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created,postVm);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        [Route("delete")]
         [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            HttpResponseMessage response = null;
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    //deleteElementImage
                    IEnumerable<PostImage> listPostImage=_postImageService.getAllByPostId(id);
                    foreach(var item in listPostImage)
                    {
                        DeleteElementImage(item.Path);
                    }
                    //delete ThubnailImage
                    if (_postService.GetById(id).Image!= null)
                    {
                        DeleteElementImage(_postService.GetById(id).Image.ToString());
                    }
                                   
                    //deletePostImage
                    _postImageService.DeleteMultiByPostId(id);
                    _postImageService.SaveChange();
                    //delete post
                    _postService.Delete(id);

                    _postService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        private void DeleteElementImage(string path)
        {
            string pathMap = HttpContext.Current.Server.MapPath(path);
            if (!string.IsNullOrEmpty(pathMap))
                File.Delete(pathMap);
        }
    }
}