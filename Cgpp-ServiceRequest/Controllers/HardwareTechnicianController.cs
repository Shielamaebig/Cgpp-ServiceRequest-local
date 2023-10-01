using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Controllers
{
    public class HardwareTechnicianController : Controller
    {
        private ApplicationDbContext _db;
        
        public HardwareTechnicianController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: HardwareTechnician
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult MyreportList()
        {
            return View();
        }
        public ActionResult MichChart()
        {
            return View();
        }
    }
}