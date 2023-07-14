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
    public class FindingApiController : ApiController
    {
        public ApplicationDbContext _db;

        public FindingApiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [HttpGet]
        [Route("api/get/findings")]
        public IHttpActionResult GetFindings()
        {
            var findings = _db.Finding.ToList().Select(Mapper.Map<Finding, FindingDto>);
            return Ok(findings);
        }

        [HttpGet]
        [Route("api/get/findingbyId/{id}")]
        public IHttpActionResult GetFindingsbyId(int id) 
        {
            var finds = _db.Finding.SingleOrDefault(x => x.Id == id);
            if (finds == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Finding, FindingDto>(finds));
        }

        [HttpPost]
        [Route("api/finding/save")]
        public IHttpActionResult saveFinding(FindingDto findingDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var finds = Mapper.Map<FindingDto, Finding>(findingDto);
            if (_db.Finding.Any(d => d.Name == finds.Name || d.Name == null))
            {
                return BadRequest("Findings name already Exist");
            }

            Finding find = new Finding();
            findingDto.Id = finds.Id;
            findingDto.Name = finds.Name;
            _db.Finding.Add(finds);
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + finds.Id), findingDto);
        }

        [HttpPut]
        [Route("api/update/finding/{id}")]
        public IHttpActionResult UpdateFinding(int id, FindingDto findingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var findingInDB = _db.Finding.SingleOrDefault(x=>x.Id == id);
            if (findingInDB == null)
            {
                return NotFound();
            }
            Mapper.Map(findingDto, findingInDB);
            findingInDB.Id = id;
            _db.SaveChanges();
            return Ok();
        }
    }
}
