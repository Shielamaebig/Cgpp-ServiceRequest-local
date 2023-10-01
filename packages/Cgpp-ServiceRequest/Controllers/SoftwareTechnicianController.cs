using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Controllers
{
    public class SoftwareTechnicianController : Controller
    {
        private ApplicationDbContext _db;

        public SoftwareTechnicianController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: SoftwareTechnician
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
                            
    }
}