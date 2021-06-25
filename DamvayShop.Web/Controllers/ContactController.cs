using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DamvayShop.Model.Models;
using DamvayShop.Service;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Controllers
{
    public class ContactController : Controller
    {
        private ISupportOnlineService _supportService;
        public ContactController(ISupportOnlineService supportService)
        {
            this._supportService = supportService;
        }
        // GET: Contact
        public ActionResult Index()
        {
            SupportOnline supportOnlineDb = _supportService.Get();
            SupportOnlineViewModel supportOnlineVm = Mapper.Map<SupportOnlineViewModel>(supportOnlineDb);
            return View(supportOnlineVm);
        }
    }
}