using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.ViewModels;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Controllers
{
    public class HardwareUploadsController : Controller
    {
        private ApplicationDbContext _context;
        public HardwareUploadsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: HardwareUploads

    }
}