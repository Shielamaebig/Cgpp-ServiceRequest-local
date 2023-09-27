using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cgpp_ServiceRequest.Models;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Controllers
{
    public class HardwareRequestController : Controller
    {
        private ApplicationDbContext _db;

        public HardwareRequestController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: HardwareRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HardwareRequestList()
        {
            var hardwareUploads = _db.HardwareUploads.Include(x => x.HardwareRequest).ToList();
            return View(hardwareUploads);
        }
        public ActionResult RequestList()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult Admin2()
        {
            return View();
        }
        public ActionResult SuperAdmin()
        {
            return View();
        }
        public ActionResult Developer()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult ManualRequest()
        {
            return View();
        }
        public ActionResult ManualRequest2()
        {
            return View();
        }
        public ActionResult AdminManual()
        {
            return View();
        }
        public ActionResult AdminManual2()
        {
            return View();
        }
    }
}