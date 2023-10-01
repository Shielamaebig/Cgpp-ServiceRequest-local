using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Controllers
{
    public class DivisionsController : Controller
    {
        private ApplicationDbContext _db;

        public DivisionsController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: Divisions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDivision()
        {
            List<Divisions> divisions = _db.Divisions.Include(m => m.Departments).ToList();
            return Json(new { data = divisions }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult updateData(int id, Divisions divisions)
        {
            var div = _db.Divisions.Where(m => m.Id == divisions.Id).FirstOrDefault();
            div.Name = divisions.Name;
            div.Departments = divisions.Departments;
            _db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDepartment()
        {

            return Json(_db.Departments.Select(x => new
            {
                DepartmentId = x.Id,
                DepartmentName = x.Name
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEdit(int Id)
        {
            var divisionList = _db.Divisions.Where(m => m.Id == Id).FirstOrDefault();
            return Json(divisionList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var data = _db.Divisions.FirstOrDefault(x => x.Id == Id);
            _db.Divisions.Remove(data);
            _db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DivisionList()
        {
            return View();
        }
    }
}