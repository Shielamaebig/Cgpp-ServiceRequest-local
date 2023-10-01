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
        public ActionResult UserRequestList()
        {
            return View();
        }
        public ActionResult AdminRequestList()
        {
            return View();
        }
        public ActionResult ProgressList()
        {
            return View();
        }
        public ActionResult ResolvedList()
        {
            return View();
        }
        public ActionResult VerifiedList()
        {
            return View();
        }
        public ActionResult HardwareRequestList()
        {
            return View();
        }
        public ActionResult Approving()
        {
            return View();
        }
        public ActionResult ApprovalList()
        {
            return View();
        }
        public ActionResult OpenList()
        {
            return View();
        }
        public ActionResult Resolved()
        {
            return View();
        }
        public ActionResult AdminIndex()
        {
            return View();
        }
        public ActionResult Progress()
        {
            return View();
        }
        public ActionResult AllRequest()
        {
            return View();
        }
        public ActionResult TechnicianReports()
        {
            return View();
        }
        public ActionResult TechnicianUpload()
        {
            return View();
        }
        public ActionResult VerifiedReport()
        {
            return View();
        }
        public ActionResult ApprovedReport()
        {
            return View();
        }
        public ActionResult HardwareTaskList()
        {
            return View();
        }
        public ActionResult PendingRequest()
        {
            return View();
        }
        public ActionResult AllRequestDivision()
        {
            return View();
        }
    }
}