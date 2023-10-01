using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;
using Microsoft.AspNet.Identity;

namespace Cgpp_ServiceRequest.Controllers
{
    public class EventListController : Controller
    {
        private ApplicationDbContext db;

        public EventListController()
        {
            db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        // GET: EventList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Event()
        {
            return View();
        }
        public JsonResult GetEvents()
        {
            var events = db.FullCalendars.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SaveEvent(FullCalendar fullCalendar)
        {
            var status = false;
            if (fullCalendar.Id >0)
            {
                var fc = db.FullCalendars.Where(a =>a.Id == fullCalendar.Id).FirstOrDefault();
                if (fc != null)
                {
                    fc.Subject = fullCalendar.Subject;
                    fc.Description = fullCalendar.Description;
                    fc.Start = fullCalendar.Start;
                    fc.End = fullCalendar.End;
                    fc.IsFullDay = fullCalendar.IsFullDay;
                    fc.ThemeColor = fullCalendar.ThemeColor;
                    fc.Name = User.Identity.GetFirstName() + " " + User.Identity.GetLastName();
                }
            }
            else
            {
                fullCalendar.Name = User.Identity.GetFullName();
                db.FullCalendars.Add(fullCalendar);
            }
            db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFirstName() + " " + User.Identity.GetLastName(),
                ActivityMessage = "Added A Event in Calendar",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            db.SaveChanges();
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        public JsonResult DeleteEvent(int Id)
        {
            var status = false;

            var v = db.FullCalendars.Where(a => a.Id == Id).FirstOrDefault();
            if (v != null)
            {
                db.FullCalendars.Remove(v);
                db.LoginActivity.Add(new LoginActivity()
                {
                    UserName = User.Identity.GetFirstName() + " " + User.Identity.GetLastName(),
                    ActivityMessage = "Remove Event in Calendar",
                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                    Email = User.Identity.GetUserName(),

                });
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}