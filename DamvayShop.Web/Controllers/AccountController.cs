using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DamvayShop.Model.Models;
using DamvayShop.Web.App_Start;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerVm)
        {
            if (ModelState.IsValid)
            {
                AppUser newAppUseDb = new AppUser();
                newAppUseDb.UserName = registerVm.UserName;
                newAppUseDb.Email = registerVm.Email;
                newAppUseDb.BirthDay = DateTime.Now;
                newAppUseDb.Id = Guid.NewGuid().ToString();
                var result = await UserManager.CreateAsync(newAppUseDb, registerVm.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRolesAsync(newAppUseDb.Id,new string[] {"User"});

                    ViewData["SuccessMgs"] = "Bạn đã đăng ký thành công";

                }
                else
                {
                    ModelState.AddModelError("","Tài khoản đã tồn tại");
                    
                }
            }
         
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Login(LoginViewModel model)
        {
           
            if (!string.IsNullOrEmpty(model.UserName)&&!string.IsNullOrEmpty(model.Password))
            {
                AppUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    string url =Request.UrlReferrer.ToString();
                    url = url.Split('/').Last();
                    if (url != "register.html")
                    {
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                                  
                }
                else
                {
                    TempData["ErrorLogin"] = "Tài khoản không đúng";
                    return Redirect("/register.html");
                }    
            }
            else
            {
                TempData["ErrorLogin"] = "Bạn phải nhập đủ thông tin";
                return Redirect("/register.html");
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            return Redirect("/");
        }


    }
}