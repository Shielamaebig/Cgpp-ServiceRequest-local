using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Controllers
{
    public class DepartmentsController : Controller
    {
        // GET: Departments
        private ApplicationDbContext _db;

        public DepartmentsController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult GetEdit(int Id)
        {
            var dprtmnt = _db.Departments.Where(m => m.Id == Id).FirstOrDefault();
            return Json(dprtmnt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Save(Departments department)
        {
            if (department.Id > 0)
            {
                var dept = _db.Departments.Where(m => m.Id == department.Id).FirstOrDefault();
                dept.Name = department.Name;
                _db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var deptInDb = new Departments();
                deptInDb.Name = department.Name;
                deptInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy");
                _db.Departments.Add(department);
                _db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var data = _db.Departments.FirstOrDefault(x => x.Id == id);
            _db.Departments.Remove(data);
            _db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DepartmentList()
        {
            return View();
        }
    }
}