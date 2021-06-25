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
    public class ProductController : Controller
    {
        private ITagService _tagService;
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IProductImageService _productImageService;
        private IProductQuantityService _productQuantityService;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IProductImageService productImageService,
          ITagService tagService, IProductQuantityService productQuantityService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._productImageService = productImageService;
            this._tagService = tagService;
            this._productQuantityService = productQuantityService;
        }
        // GET: ProductCategory
      
        public ActionResult Index(int id, int page = 1, string sort = "")
        {
            ProductCategory category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategoryViewModel>(category);
            ViewBag.Sort = sort;
            int pageSize = Common.CommonConstant.PageSize;
            int totalRow = 0;
            IEnumerable<Product> listProductDb = _productService.GetAllByCategoryPaging(id, page, pageSize, sort, out totalRow);
            IEnumerable<ProductViewModel> listProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listProductDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<ProductViewModel> pagination = new PaginationClient<ProductViewModel>()
            {
                TotalPage = totalPage,
                TotalRows = totalRow,
                PageDisplay = Common.CommonConstant.PageDisplay,
                Items= listProductVm,
                PageIndex=page,
                PageSize=pageSize,

            };


            return View(pagination);
        } 
        public JsonResult GetListProductByName(string prodcuctName)
        {
           IEnumerable<string> listProductName = _productService.GetProductName(prodcuctName);
            return Json(new
            {
                data = listProductName
            },JsonRequestBehavior.AllowGet);
                
            
        }

        public ActionResult SearchProduct(string productName, int page = 1, string sort="")
        {
            int pageSize = Common.CommonConstant.PageSize;
            int totalRow = 0;
            ViewBag.Sort = sort;
            ViewBag.ProductName = productName;
            IEnumerable<Product> listProductDb = _productService.GetAllByNamePaging(productName, page, pageSize, sort, out totalRow);
            IEnumerable<ProductViewModel> listProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listProductDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<ProductViewModel> pagination = new PaginationClient<ProductViewModel>()
            {
                PageDisplay=Common.CommonConstant.PageDisplay,
                PageIndex=page,
                PageSize=pageSize,
                TotalPage=totalPage,
                Items=listProductVm,
                TotalRows=totalRow,
            };
            return View(pagination);
        }

        public ActionResult Detail(int id)
        {
            Product productDb = _productService.GetById(id);
            ProductViewModel productVm = Mapper.Map<ProductViewModel>(productDb);
            IEnumerable<Size> sizeDb = _productQuantityService.GetSizeByProductId(id);
            IEnumerable<SizeViewModel> sizeVm = Mapper.Map<IEnumerable<SizeViewModel>>(sizeDb);

            IEnumerable<Product> listProductDb = _productService.GetProductRelate(productVm.CategoryID);
            IEnumerable<ProductViewModel> listProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listProductDb);
            IEnumerable<ProductImage> listProductImageDb = _productImageService.GetProductImageByProdutID(id);
            IEnumerable<ProductImageViewModel> listProductImageVm = Mapper.Map<IEnumerable<ProductImageViewModel>>(listProductImageDb);
            IEnumerable<Tag> listTagDb = _tagService.GetTagByProductId(id);
            IEnumerable<TagViewModel> listTagVm = Mapper.Map<IEnumerable<TagViewModel>>(listTagDb);
            ViewBag.TagProducts = listTagVm;
            ViewBag.ProductCategory = productDb.ProductCategory;

            ProductDetailViewModel ProductDetail = new ProductDetailViewModel()
            {
                ListProductImageVm = listProductImageVm,
                ListProductVm = listProductVm,
                ProductVm = productVm,
                SizeVm=sizeVm,               
            };
            return View(ProductDetail);

        }

        public ActionResult HotProduct(int page=1)
        {
            int pageSize = Common.CommonConstant.PageSize;
            int totalRow = 0;
            IEnumerable<Product> listHotProductDb = _productService.GetAllHotProduct(page,pageSize,out totalRow);
            IEnumerable<ProductViewModel> listHotProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listHotProductDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<ProductViewModel> paginnation = new PaginationClient<ProductViewModel>()
            {
                PageSize=pageSize,
                PageDisplay=Common.CommonConstant.PageDisplay,
                PageIndex =page,
                TotalPage=totalPage,
                TotalRows=totalRow,
                Items=listHotProductVm,
            };
            return View(paginnation);

        }

        public ActionResult PromotionProduct(int page = 1)
        {
            int pageSize = Common.CommonConstant.PageSize;
            int totalRow = 0;
            IEnumerable<Product> listProductDb = _productService.GetAllPromotionProduct(page, pageSize, out totalRow);
            IEnumerable<ProductViewModel> listProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listProductDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<ProductViewModel> pagination = new PaginationClient<ProductViewModel>()
            {
                PageIndex = page,
                PageSize = pageSize,
                PageDisplay = Common.CommonConstant.PageDisplay,
                TotalPage = totalPage,
                Items = listProductVm,
                TotalRows=totalRow
            };
            return View(pagination);

        }
        public ActionResult Tag(string tagId, int page=1)
        {
            int pageSize = Common.CommonConstant.PageSize;
            int totalRow = 0;
            IEnumerable<Product> listProductDb = _productService.GetAllByTagPaging(tagId, page, pageSize, out totalRow);
            IEnumerable<ProductViewModel> listProductVm = Mapper.Map<IEnumerable<ProductViewModel>>(listProductDb);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            PaginationClient<ProductViewModel> pagination = new PaginationClient<ProductViewModel>()
            {
                PageIndex=page,
                PageDisplay=Common.CommonConstant.PageDisplay,
                PageSize=pageSize,
                TotalPage=totalPage,
                TotalRows=totalRow,
                Items=listProductVm,
            };
            ViewBag.ProductTag =Mapper.Map<TagViewModel>(_tagService.GetDetail(tagId));
            return View(pagination);

        }
    }
}