using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;
using DamvayShop.Web.Providers;

namespace DamvayShop.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        private IProductService _productService;
        private IProductImageService _productImageService;

        public ProductController(IProductService productService, IErrorService errorService, IProductImageService productImageService) : base(errorService)
        {
            this._productService = productService;
            this._productImageService = productImageService;
        }
        [Route("getallparents")]
        [HttpGet]
        [Permission(Action = "Read", Function = "PRODUCT")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _productService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [Route("getall")]
        [HttpGet]
        [Permission(Action = "Read", Function = "PRODUCT")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int? categoryId, int page, int pageSize = 20, string filterHotPromotion = "", string keyword = "")
        {
            Func<HttpResponseMessage> func = () =>
            {
                int totalRow = 0;
                IEnumerable<Product> listProductDb = _productService.GetAll(categoryId, filterHotPromotion, keyword);
                totalRow = listProductDb.Count();
                IEnumerable<Product> query = listProductDb.OrderByDescending(x => x.CreateDate).Skip(pageSize * (page - 1)).Take(pageSize);
                IEnumerable<ProductViewModel> listProduct = Mapper.Map<IEnumerable<ProductViewModel>>(query);
                PaginationSet<ProductViewModel> pagination = new PaginationSet<ProductViewModel>()
                {
                    TotalRows = totalRow,
                    PageIndex = page,
                    PageSize = pageSize,
                    Items = listProduct,
                };
                return request.CreateResponse(HttpStatusCode.OK, pagination);
            };
            return CreateHttpResponse(request, func);
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        [Permission(Action = "Read", Function = "PRODUCT")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            Func<HttpResponseMessage> Fuc = () =>
            {
                Product productDb = _productService.GetById(id);
                ProductViewModel productVm = Mapper.Map<ProductViewModel>(productDb);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productVm);
                return response;
            };
            return CreateHttpResponse(request, Fuc);
        }

        [Route("add")]
        [HttpPost]
        [Permission(Action = "Create", Function = "PRODUCT")]
        public HttpResponseMessage post(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    Product productDb = new Product();
                    productDb.UpdateProduct(productVm);
                    productDb.CreateDate = DateTime.Now;
                    productDb.UpdatedDate = DateTime.Now;
                    productDb.CreateBy = User.Identity.Name.ToString();
                    var product = _productService.Add(productDb);
                    _productService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created, productVm);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Permission(Action = "Update", Function = "PRODUCT")]
        public HttpResponseMessage put(HttpRequestMessage request, ProductViewModel productVm)
        {
            HttpResponseMessage response = null;
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    Product productDb = _productService.GetById(productVm.ID);
                    productDb.UpdateProduct(productVm);
                    productDb.UpdatedDate = DateTime.Now;
                    _productService.Update(productDb);
                    _productService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created, productVm);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Permission(Action = "Delete", Function = "PRODUCT")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            HttpResponseMessage response = null;
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    List<ProductImage> listProductImage = _productImageService.GetProductImageByProdutID(id);
                    _productService.Delete(id);
                    _productService.SaveChanges();
                    for (int i = 0; i < listProductImage.Count(); i++)
                    {
                        DeleteElementImage(listProductImage[i].Path);
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Permission(Action = "Delete", Function = "PRODUCT")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
        {
            Func<HttpResponseMessage> Func = () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    var listProduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    List<string> listPath = new List<string>() { };

                    foreach (var productID in listProduct)
                    {
                        List<ProductImage> listProductImage = _productImageService.GetProductImageByProdutID(productID);
                        for (int i = 0; i < listProductImage.Count(); i++)
                        {
                            listPath.Add(listProductImage[i].Path);
                        }
                        _productService.Delete(productID);
                    }                  
                    _productService.SaveChanges();
                    for (int i = 0; i < listPath.Count(); i++)
                    {
                        DeleteElementImage(listPath[i]);
                    }

                    response = request.CreateResponse(HttpStatusCode.Created, listProduct.Count());
                }
                else
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                return response;
            };
            return CreateHttpResponse(request, Func);
        }

        [Route("thumnailImage")]
        [HttpPut]
        [Permission(Action = "Update", Function = "PRODUCT")]
        public HttpResponseMessage UpdateThumbnail(HttpRequestMessage request, int productId)
        {
            return CreateHttpResponse(request, () =>
             {
                 try
                 {
                     ProductImage ProductImage = _productImageService.GetAll(productId).FirstOrDefault();
                     Product productDb = _productService.GetById(productId);
                     productDb.ThumbnailImage = ProductImage.Path;
                     _productService.Update(productDb);
                     _productService.SaveChanges();
                     return request.CreateResponse(HttpStatusCode.Created, productId);
                 }
                 catch
                 {
                     return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Sản phẩm không có ảnh");
                 }
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