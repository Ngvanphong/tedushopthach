using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DamvayShop.Common;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProductCategoryService _productCategoryService;
        private IPostCategoryService _postCategoryService;

        private IProductService _productService;
        private IPostService _postService;
        private ISlideService _slideService;

        private ITagService _tagService;
        private ISupportOnlineService _supportOnline;

        private IFooterService _footerService;

        private ISystemConfigService _systemConfigService;

        public HomeController(IProductCategoryService productCatgoryService, IPostCategoryService postCategoryService,
            IProductService productService, IPostService postService, ISlideService slideService, ITagService tagService, ISupportOnlineService supportOnline,
            IFooterService footerService, ISystemConfigService systemConfigService)
        {
            this._productCategoryService = productCatgoryService;
            this._postCategoryService = postCategoryService;
            this._postService = postService;
            this._productService = productService;
            this._slideService = slideService;
            this._tagService = tagService;
            this._supportOnline = supportOnline;
            this._footerService = footerService;
            this._systemConfigService = systemConfigService;
        }

  
        public ActionResult Index()
        {

            IndexViewModel indexVm = new IndexViewModel();         
            IEnumerable<Product> listHotProductDb = _productService.GetHotProduct();
            IEnumerable<ProductViewModel> listHotProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listHotProductDb);
            indexVm.productHotVm = listHotProductVm;
            IEnumerable<Product> listPromotionProductDb = _productService.GetPromotionProduct();
            IEnumerable<ProductViewModel> listPromotionVm = Mapper.Map<IEnumerable<ProductViewModel>>(listPromotionProductDb);
            indexVm.productPromotionVm = listPromotionVm;

            IEnumerable<Slide> listSlideDb = _slideService.GetAll().Where(x=>x.Status==true);
            IEnumerable<SlideViewModel> listSlideVm = Mapper.Map<IEnumerable<SlideViewModel>>(listSlideDb);
            indexVm.slideVm = listSlideVm;

            IEnumerable<Post> listPostDb = _postService.GetAll().Where(x => x.HomeFlag == true).OrderByDescending(x => x.CreateDate).Take(3);
            IEnumerable<PostViewModel> listPostVm = Mapper.Map<IEnumerable<PostViewModel>>(listPostDb);

            try
            {
                indexVm.Title = _systemConfigService.GetByCode(CommonConstant.Title);
                indexVm.MetaKeyword= _systemConfigService.GetByCode(CommonConstant.MetaKeyword);
                indexVm.MetaDiscription = _systemConfigService.GetByCode(CommonConstant.MetaDiscription);
            }
            catch
            {

            }
            indexVm.postVm = listPostVm;

            return View(indexVm);
        }
        
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            FooterCommon footVm = new FooterCommon();
            IEnumerable<ProductCategory> listAll = _productCategoryService.GetAll();
            IEnumerable<ProductCategory> listParent = listAll.Where(x => x.ParentID == null);
            List<ProductCategory> listChild = new List<ProductCategory> { };
            foreach(var item in listParent)
            {
                
                var list = listAll.Where(x => x.ParentID == item.ID);
                if (list.Count() == 0)
                {
                    listChild.Add(item);
                }
                else
                {
                    listChild.AddRange(list);
                }
                
            }
            listChild = listChild.OrderBy(x => x.Name).Take(9).ToList();
            IEnumerable<ProductCategoryViewModel> listCategoryVm = Mapper.Map<IEnumerable<ProductCategoryViewModel>>(listChild);
            footVm.listCategoryProduct = listCategoryVm;

            IEnumerable<Tag> listTagProdut = _tagService.GetAll().Where(x => x.Type == Common.CommonConstant.ProductTag.ToString()).OrderBy(x => x.Name).Take(18);
            IEnumerable<TagViewModel> listTagVm = Mapper.Map<IEnumerable<TagViewModel>>(listTagProdut);
            footVm.tagVm = listTagVm;

            SupportOnline supportDb = _supportOnline.Get();
            SupportOnlineViewModel supportVm = Mapper.Map<SupportOnlineViewModel>(supportDb);
            footVm.supportOnlineVm = supportVm;

            Footer footerDb = _footerService.GetAll();
            FooterViewModel footerVm = Mapper.Map<FooterViewModel>(footerDb);
            footVm.footerVm = footerVm;
            
            return PartialView(footVm);
        }

        [ChildActionOnly]      
        public ActionResult Header()
        {
            int countShopping = 0;
            if (Session[Common.CommonConstant.CountShopping] != null)
            {
                countShopping = (int)Session[Common.CommonConstant.CountShopping];
            }
            else
            {
                Session[Common.CommonConstant.CountShopping] = new int();
            }

            HeaderViewModel headerVm =new HeaderViewModel()
            {

            };
            headerVm.CountShopping = countShopping;
            

            return PartialView(headerVm);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult CategoryHeader()
        {
            CategoryHeaderViewModel categogyHeaderVm = new CategoryHeaderViewModel();
            IEnumerable<ProductCategory> productCategoryDb = _productCategoryService.GetAll().OrderBy(x=>x.HomeOrder);
            IEnumerable<ProductCategoryViewModel> productCategoryVm = Mapper.Map<IEnumerable<ProductCategoryViewModel>>(productCategoryDb);
            IEnumerable<PostCategory> postCategoryDb = _postCategoryService.GetAll();
            IEnumerable<PostCategoryViewModel> postCategoryVm = Mapper.Map<IEnumerable<PostCategoryViewModel>>(postCategoryDb);

            categogyHeaderVm.productCategoryVm = productCategoryVm;
            categogyHeaderVm.postCategoryVm = postCategoryVm;

            return PartialView(categogyHeaderVm);
        }

    }
}