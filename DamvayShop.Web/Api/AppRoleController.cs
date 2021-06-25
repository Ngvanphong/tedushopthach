using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Api
{
    [RoutePrefix("api/appRole")]
    [Authorize]
    public class AppRoleController : ApiControllerBase
    {
        private IFunctionService _functionService;
        private IPermissionService _permissionService;
        private IUserRoleServie _userRoleService;

        public AppRoleController(IErrorService errorService, IFunctionService functionService, IPermissionService permissionService, IUserRoleServie userRoleService) : base(errorService)
        {
            this._functionService = functionService;
            this._permissionService = permissionService;
            this._userRoleService = userRoleService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                int totalRows = 0;
                var query = AppRoleManager.Roles;
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.Description.Contains(filter));
                totalRows = query.Count();
                var model = query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(model);
                PaginationSet<ApplicationRoleViewModel> pagination = new PaginationSet<ApplicationRoleViewModel>
                {
                    PageIndex = page,
                    TotalRows = totalRows,
                    PageSize = pageSize,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagination);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppRoleManager.Roles.ToList();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                AppRole roleDb = AppRoleManager.FindById(id);
                ApplicationRoleViewModel roleVm = Mapper.Map<ApplicationRoleViewModel>(roleDb);
                response = request.CreateResponse(HttpStatusCode.OK, roleVm);
                return response;
            });
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                _permissionService.DeleteAllByRoleID(id);
                _permissionService.SaveChange();
                _userRoleService.Delete(id);
                _userRoleService.SaveChange();
                AppRole role = AppRoleManager.FindById(id);
                AppRoleManager.Delete(role);
                response = request.CreateResponse(HttpStatusCode.OK, id);
                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    AppRole newRoleDb = new AppRole();
                    newRoleDb.UpdateApplicationRole(applicationRoleViewModel, "add");
                    AppRoleManager.Create(newRoleDb);
                    response = request.CreateResponse(HttpStatusCode.Created, newRoleDb);
                }

                return response;
            });
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            HttpResponseMessage response = null;
            return CreateHttpResponse(request, () =>
            {
                if (ModelState.IsValid)
                {
                    AppRole roleDb = AppRoleManager.FindById(applicationRoleViewModel.Id);
                    roleDb.UpdateApplicationRole(applicationRoleViewModel, "update");
                    AppRoleManager.Update(roleDb);
                    response = request.CreateResponse(HttpStatusCode.Created, roleDb);
                }
                return response;
            });
        }

        [Route("getAllPermission")]
        [HttpGet]
        public HttpResponseMessage GetAllPermission(HttpRequestMessage request, string functionId)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                List<PermissionViewModel> permisstions = new List<PermissionViewModel>();
                List<AppRole> roles = AppRoleManager.Roles.Where(r => r.Name != "Admin").ToList();
                var listPermission = _permissionService.GetByFunctionId(functionId);
                if (listPermission.Count == 0)
                {
                    foreach (var item in roles)
                    {
                        permisstions.Add(new PermissionViewModel()
                        {
                            RoleId = item.Id,
                            CanCreate = false,
                            CanUpdate = false,
                            CanDelete = false,
                            CanRead = false,
                            AppRole = new ApplicationRoleViewModel()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Description = item.Description,
                            }
                        });
                    }
                }
                else
                {
                    foreach (var item in roles)
                    {
                        if (!listPermission.Any(x => x.FunctionId == item.Id))
                        {
                            permisstions.Add(new PermissionViewModel()
                            {
                                RoleId = item.Id,
                                CanCreate = false,
                                CanUpdate = false,
                                CanDelete = false,
                                CanRead = false,
                                AppRole = new ApplicationRoleViewModel()
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Description = item.Description,
                                }
                            });
                        }
                        permisstions = Mapper.Map<List<PermissionViewModel>>(listPermission);
                    }
                };
                response = request.CreateResponse(HttpStatusCode.OK, permisstions);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [HttpPost]
        [Route("savePermission")]
        public HttpResponseMessage SavePermission(HttpRequestMessage request, SavePermissionRequest data)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    _permissionService.DeleteAll(data.FunctionId);
                    foreach (var item in data.Permissions)
                    {
                        Permission permission = new Permission();
                        permission.UpdatePermission(item);
                        permission.FunctionId = data.FunctionId;
                        _permissionService.Add(permission);
                    }
                    var functions = _functionService.GetAllWithParentID(data.FunctionId);
                    if (functions.Any())
                    {
                        foreach (var item in functions)
                        {
                            _permissionService.DeleteAll(item.ID);
                            foreach (var per in data.Permissions)
                            {
                                var permission = new Permission()
                                {
                                    FunctionId = item.ID,
                                    RoleId = per.RoleId,
                                    CanCreate = per.CanCreate,
                                    CanRead = per.CanRead,
                                    CanDelete = per.CanDelete,
                                    CanUpdate = per.CanUpdate,
                                };
                                _permissionService.Add(permission);
                            }
                        }
                    }
                    _permissionService.SaveChange();
                    response = request.CreateResponse(HttpStatusCode.OK, "Lưu quyền thành công");
                }
                return response;
            };
            return CreateHttpResponse(request, func);
        }
    }
}