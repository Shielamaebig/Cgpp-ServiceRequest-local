using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cgpp_ServiceRequest.Controllers
{
    public class LoginActivityController : Controller
    {
        // GET: LoginActivity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Activity()
        {
            return View();
        }
        public ActionResult RequestActivities()
        {
            return View();
        }
        public ActionResult SoftwareRequests()
        {
            return View();
        }
        public ActionResult HardwareRequests()
        {
            return View();
        }
    }
}