using AutoMapper;
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
    [RoutePrefix("api/productQuantity")]
    public class ProductQuantityController : ApiControllerBase
    {
        private IProductQuantityService _productQuantityService;

        public ProductQuantityController(IErrorService errorService, IProductQuantityService productQuantityService) : base(errorService)
        {
            this._productQuantityService = productQuantityService;
        }

        [Route("getsizes")]
        [HttpGet]
        public HttpResponseMessage GetSizes(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                IEnumerable<Size> listSizeDb = _productQuantityService.GetListSize();
                IEnumerable<SizeViewModel> listSizeVm = Mapper.Map<IEnumerable<SizeViewModel>>(listSizeDb);
                listSizeVm = listSizeVm.OrderBy(x => x.Name);
                return request.CreateResponse(HttpStatusCode.OK, listSizeVm);
            });
        }

        [Route("sizesdetail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetSizes(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var sizeDb = _productQuantityService.GetSizeById(id);
                var sizeVm = Mapper.Map<SizeViewModel>(sizeDb);
                return request.CreateResponse(HttpStatusCode.OK, sizeVm);
            });
        }

        [Route("addsizes")]
        [HttpPost]
        public HttpResponseMessage AddSizes(HttpRequestMessage request, SizeViewModel sizeVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    Size sizeDb = new Size();
                    sizeDb.UpdateSize(sizeVm);
                    _productQuantityService.AddSize(sizeDb);
                    _productQuantityService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, sizeVm);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
            });
        }

        [Route("updatesizes")]
        [HttpPut]
        public HttpResponseMessage UpdateSizes(HttpRequestMessage request, SizeViewModel sizeVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    var sizeDb = _productQuantityService.GetSizeById(sizeVm.ID);
                    sizeDb.UpdateSize(sizeVm);
                    _productQuantityService.UpdateSize(sizeDb);
                    _productQuantityService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, sizeVm);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
            });
        }

        [Route("deletesize")]
        [HttpDelete]
        public HttpResponseMessage DeleteSizes(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    _productQuantityService.DeleteSize(id);
                    _productQuantityService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int productId, int? sizeId)
        {
            return CreateHttpResponse(request, () =>
            {
                IEnumerable<ProductQuantity> listProductQuatityDb = _productQuantityService.GetAll(productId, sizeId);
                IEnumerable<ProductQuantityViewModel> listProductVm = Mapper.Map<IEnumerable<ProductQuantityViewModel>>(listProductQuatityDb);
                return request.CreateResponse(HttpStatusCode.OK, listProductVm);
            });
        }
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductQuantityViewModel productQuantityVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    if (_productQuantityService.CheckExist(productQuantityVm.ProductId, productQuantityVm.SizeId))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Size cho sản phẩm này đã tồn tại");
                    }
                    else
                    {
                        ProductQuantity productQuantityDb = new ProductQuantity();
                        productQuantityDb.UpdateProductQuantity(productQuantityVm);
                        _productQuantityService.Add(productQuantityDb);
                        _productQuantityService.SaveChange();
                        return request.CreateResponse(HttpStatusCode.Created, productQuantityVm);
                    }
                   
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
               
            });
        }
        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductQuantityViewModel productQuantityVm)
        {
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    ProductQuantity productQuantityDb = _productQuantityService.GetSingle(productQuantityVm.ProductId, productQuantityVm.SizeId);
                    productQuantityDb.UpdateProductQuantity(productQuantityVm);
                    _productQuantityService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.Created, productQuantityVm);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            });
           
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int productId, int sizeId)
        {
           return CreateHttpResponse(request, () =>
            {
                _productQuantityService.Delete(productId, sizeId);
                _productQuantityService.SaveChange();
                return request.CreateResponse(HttpStatusCode.OK, "Xóa thành công");
            });
        }
    }
}