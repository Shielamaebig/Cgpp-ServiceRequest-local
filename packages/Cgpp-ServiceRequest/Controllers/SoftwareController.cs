using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cgpp_ServiceRequest.Controllers
{
    public class SoftwareController : Controller
    {
        // GET: Software
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SoftwareRequest()
        {
            return View();
        }
    }
}