using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class FtpApiController : ApiController
    {
        private ApplicationDbContext _ctx;

        public FtpApiController()
        {
            _ctx = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
        }
        [Route("api/ftp/get")]
        public IHttpActionResult GetDepartments()
        {
            var ftps = _ctx.Ftp.ToList().Select(Mapper.Map<Ftp, FtpDto>);
            return Ok(ftps);
        }
        [HttpPost]
        [Route("api/ftp/post")]

        public IHttpActionResult PostFtp (FtpDto ftpDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest();
            var ftpcreds = Mapper.Map<FtpDto, Ftp>(ftpDto);

            if (_ctx.Ftp.Any(x=>x.FtpHost == ftpcreds.FtpHost || x.FtpHost == null))
            {
                return BadRequest("Host name already Exist");
            }
            ftpcreds.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            Ftp fc = new Ftp();
            fc.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            ftpDto.Id = ftpcreds.Id;
            _ctx.Ftp.Add(ftpcreds);
            _ctx.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + ftpcreds.Id), ftpDto);

        }

        [HttpGet]
        [Route("api/ftp/getbyId/{id}")]
        public IHttpActionResult GetFtp(int id)
        {
            var ftps = _ctx.Ftp.SingleOrDefault(a => a.Id == id);
            if (ftps == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Ftp, FtpDto>(ftps));
        }

        [HttpPut]
        [Route("api/ftp/update/{id}")]
        public IHttpActionResult PutFtp(int id, FtpDto ftpDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ftpInDb = _ctx.Ftp.SingleOrDefault(c=>c.Id == id);
            if (ftpInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(ftpDto, ftpInDb);
            ftpInDb.Id = id;
            ftpInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            ftpDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _ctx.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/ftp/Remove/{id}")]
        public IHttpActionResult DeleteFtp(int id)
        {
            var ftpInDb = _ctx.Ftp.SingleOrDefault(f => f.Id == id);
            if (ftpInDb == null) { 
                return NotFound();
            }
            _ctx.Ftp.Remove(ftpInDb);
            _ctx.SaveChanges();
            return Ok();
        }
    }
}
