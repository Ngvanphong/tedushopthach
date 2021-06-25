using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DamvayShop.Common;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Infrastructure.Core;
using DamvayShop.Web.Infrastructure.Extensions;
using DamvayShop.Web.Models;
using DamvayShop.Web.Providers;

namespace DamvayShop.Web.Api
{
    [Authorize]
    [RoutePrefix("api/appUser")]
    public class AppUserController : ApiControllerBase
    {
        private IUserRoleServie _userRoleService;

        public AppUserController(IErrorService errorService, IUserRoleServie userRoleService) : base(errorService)
        {
            this._userRoleService = userRoleService;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Permission(Action = "Read", Function = "USER")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            Func<HttpResponseMessage> func = () =>
            {
                HttpResponseMessage response = null;
                int totalRows = 0;
                var listAppUserDb = AppUserManager.Users;
                if (!string.IsNullOrEmpty(filter))
                    listAppUserDb = listAppUserDb.Where(x => x.UserName.Contains(filter) || x.FullName.Contains(filter));
                totalRows = listAppUserDb.Count();
                var query = listAppUserDb.OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize);
                IEnumerable<ApplicationUserViewModel> listApplicationUserVm = Mapper.Map<IEnumerable<ApplicationUserViewModel>>(query);
                PaginationSet<ApplicationUserViewModel> pagination = new PaginationSet<ApplicationUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRows,
                    Items = listApplicationUserVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagination);
                return response;
            };
            return CreateHttpResponse(request, func);
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Permission(Action = "Read", Function = "USER")]
        //[Authorize(Roles = "ViewUser")]
        public async Task<HttpResponseMessage> Details(HttpRequestMessage request, string id)
        {
            HttpResponseMessage response = null;
            var user = await AppUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var listRole = await AppUserManager.GetRolesAsync(user.Id);
                var userModel = Mapper.Map<ApplicationUserViewModel>(user);
                userModel.Roles = listRole;
                response = request.CreateResponse(HttpStatusCode.OK, userModel);
                return response;
            }
        }

        [HttpPost]
        [Route("add")]
        //[Authorize(Roles = "AddUser")]
        [Permission(Action = "Create", Function = "USER")]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser newAppUseDb = new AppUser();
                newAppUseDb.UpdateUser(applicationUserViewModel);
                try
                {
                    newAppUseDb.Id = Guid.NewGuid().ToString();
                    var result = await AppUserManager.CreateAsync(newAppUseDb, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var roles = applicationUserViewModel.Roles.ToArray();
                        await AppUserManager.AddToRolesAsync(newAppUseDb.Id, roles);
                        return request.CreateResponse(HttpStatusCode.Created, applicationUserViewModel);
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                    }
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            };
        }

        [HttpPut]
        [Route("update")]
        //[Authorize(Roles = "UpdateUser")]
        [Permission(Action = "Update", Function = "USER")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser userDb = await AppUserManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    if(userDb.Avatar!=applicationUserViewModel.Avatar&&userDb.Avatar!=null)
                    DeleteElementImage(userDb.Avatar);
                    userDb.UpdateUser(applicationUserViewModel);                 
                    var result = await AppUserManager.UpdateAsync(userDb);
                    if (result.Succeeded)
                    {
                        var roles = await AppUserManager.GetRolesAsync(applicationUserViewModel.Id);
                        await AppUserManager.RemoveFromRolesAsync(applicationUserViewModel.Id, roles.ToArray());
                        var selectRoles = applicationUserViewModel.Roles.ToArray();
                        selectRoles = selectRoles ?? new string[] { };
                        await AppUserManager.AddToRolesAsync(applicationUserViewModel.Id, selectRoles);
                        

                        return request.CreateResponse(HttpStatusCode.Created, applicationUserViewModel);
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadGateway, string.Join(",", result.Errors));
                    }
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }



        [HttpDelete]
        [Route("delete")]
        [Permission(Action = "Delete", Function = "USER")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            _userRoleService.DeleteByUserId(id);
            _userRoleService.SaveChange();
            AppUser user = await AppUserManager.FindByIdAsync(id);
            var result = await AppUserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                if (user.Avatar != null)
                {
                    DeleteElementImage(user.Avatar);
                }              
                return request.CreateResponse(HttpStatusCode.OK, id);
            }        
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
        }

        private void DeleteElementImage(string path)
        {
            string pathMap = HttpContext.Current.Server.MapPath(path);
            if (!string.IsNullOrEmpty(pathMap))
                File.Delete(pathMap);
        }
    }
}