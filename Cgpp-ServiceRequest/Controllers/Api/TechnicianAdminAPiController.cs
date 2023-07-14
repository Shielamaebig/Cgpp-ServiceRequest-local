using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models.Extensions;


namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class TechnicianAdminAPiController : ApiController
    {
        public ApplicationDbContext _db;

        public TechnicianAdminAPiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        //[HttpGet]
        //[Route("api/techAdmin/Get")]
        //public IHttpActionResult GetTechnician()
        //{
        //    var techAdmin = _db.TechnicianAdmins.ToList().Select(Mapper.Map<TechnicianAdmin, TechnicianAdminDto>);
        //    return Ok(techAdmin);
        //}

        //[HttpGet]
        //[Route("api/techAdmin/byId/{id}")]
        //public IHttpActionResult UpdateTechAdmin(int id)
        //{
        //    var techAdmins = _db.TechnicianAdmins.SingleOrDefault(x=> x.Id == id);
        //    if (techAdmins == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Mapper.Map<TechnicianAdmin, TechnicianAdminDto>(techAdmins));
        //}
        //[HttpPost]
        //[Route("api/techAdmin/save")]
        //public IHttpActionResult SaveFile(TechnicianAdminDto technicianAdminDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var technicianAdmin = Mapper.Map<TechnicianAdminDto, TechnicianAdmin>(technicianAdminDto);
        //    if (_db.TechnicianAdmins.Any(d => d.Name == technicianAdmin.Name || d.Name == null))
        //    {
        //        return BadRequest("Tech Admin name already Exist");
        //    }
        //    TechnicianAdmin techA = new TechnicianAdmin();
        //    technicianAdminDto.Id = technicianAdmin.Id;
        //    technicianAdminDto.Name = technicianAdmin.Name;
        //    technicianAdminDto.Email = technicianAdmin.Email;
        //    _db.TechnicianAdmins.Add(technicianAdmin);
        //    _db.SaveChanges();
        //    return Created(new Uri(Request.RequestUri + "/" + technicianAdmin.Id), technicianAdminDto);
        //}

        //[HttpPut]
        //[Route("api/update/techAdmin/{id}")]
        //public IHttpActionResult updateFile(int id, TechnicianAdminDto technicianAdminDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var techInDB = _db.TechnicianAdmins.SingleOrDefault(x=>x.Id == id);
        //    if (techInDB == null)
        //    {
        //        return NotFound();
        //    }
        //    Mapper.Map(technicianAdminDto, techInDB);
        //    techInDB.Id = id;
        //    _db.SaveChanges();
        //    return Ok();
        //}
    }
}
