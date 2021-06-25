using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
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
    [RoutePrefix("api/function")]
    public class FunctionController : ApiControllerBase
    {
        private IFunctionService _functionService;
        private IPermissionService _permissionService;

        public FunctionController(IErrorService errorService, IFunctionService functionService, IPermissionService permissionService) : base(errorService)
        {
            this._functionService = functionService;
            this._permissionService = permissionService;
        }
        [Route("getlisthierarchy")]
        [HttpGet]
        public HttpResponseMessage GetAllHierachy(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IEnumerable<Function> model;
                if (User.IsInRole("Admin"))
                {
                    model = _functionService.GetAll(string.Empty);
                }
                else
                {
                    model = _functionService.GetAllWithPermission(User.Identity.GetUserId());
                }

                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);
                var parents = modelVm.Where(x => x.Parent == null);
                foreach (var parent in parents)
                {
                    parent.ChildFunctions = modelVm.Where(x => x.ParentId == parent.ID).ToList();
                }
                response = request.CreateResponse(HttpStatusCode.OK, parents);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string filter = "")
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _functionService.GetAll(filter);

                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                Function functionDb = _functionService.Get(id);
                FunctionViewModel functionVm = Mapper.Map<FunctionViewModel>(functionDb);
                response = request.CreateResponse(HttpStatusCode.OK, functionVm);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, FunctionViewModel functionViewModel)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    Function newFunctionDb = new Function();
                    if (_functionService.CheckExistedId(functionViewModel.ID))
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID đã tồn tại");
                    }
                    else
                    {
                        if (functionViewModel.ParentId == "")
                            functionViewModel.ParentId = null;
                        newFunctionDb.UpdateFunction(functionViewModel);
                        _functionService.Create(newFunctionDb);
                        _functionService.SaveChange();
                        response = request.CreateResponse(HttpStatusCode.Created, functionViewModel);
                    }
                }
                return response;
            };
            return CreateHttpResponse(request, func);
        }
        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, FunctionViewModel functionViewModel)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    Function functionDb = _functionService.Get(functionViewModel.ID);
                    functionDb.UpdateFunction(functionViewModel);
                    _functionService.Update(functionDb);
                    _functionService.SaveChange();
                    response = request.CreateResponse(HttpStatusCode.Created, functionViewModel);
                }
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                _permissionService.DeleteAll(id);
                _permissionService.SaveChange();
                HttpResponseMessage response = null;
                _functionService.Delete(id);
                _functionService.SaveChange();
                response = request.CreateResponse(HttpStatusCode.OK, id);
                return response;
            });
        }

    }
}