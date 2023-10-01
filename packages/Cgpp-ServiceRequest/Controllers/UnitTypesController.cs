using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cgpp_ServiceRequest.Models;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Controllers
{
    public class UnitTypesController : Controller
    {
        private ApplicationDbContext _db;

        public UnitTypesController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: UnitTypes
        public ActionResult Index()
        {
            return View();
        }
    }
}