using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Controllers
{
    public class PostController : Controller
    {
        private IPostService _postService;
        private IPostCategoryService _postCategoryService;
        public PostController(IPostService postService, IPostCategoryService postCategoryService)
        {
            this._postService = postService;
            this._postCategoryService = postCategoryService;
        }
        // GET: Post
        public ActionResult Index(int id, int page=1)
        {
            int pageSize = 2;
            int totalRow = 0;

            PostCategory postCategoryDb = _postCategoryService.GetByID(id);
            PostCategoryViewModel postCategoryVm = Mapper.Map<PostCategoryViewModel>(postCategoryDb);
            ViewBag.Category = postCategoryVm;
            IEnumerable<Post> listPostDb = _postService.GetByCategoryPaging(id, page, pageSize, out totalRow);
            IEnumerable<PostViewModel> listPostVm = Mapper.Map<IEnumerable<PostViewModel>>(listPostDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<PostViewModel> pagination = new PaginationClient<PostViewModel>()
            {
                PageIndex=page,
                PageDisplay=Common.CommonConstant.PageDisplay,
                PageSize=pageSize,
                TotalPage=totalPage,
                TotalRows=totalRow,
                Items=listPostVm,
            };

            return View(pagination);
        }

        public ActionResult Detail(string postid)
        {
            Post postDb = _postService.GetById(postid);
            PostViewModel postVm = Mapper.Map<PostViewModel>(postDb);            
            return View(postVm);
        }
    }
}