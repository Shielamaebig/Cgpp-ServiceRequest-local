using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class HardwareUserControllerApi : ApiController
    {
        public ApplicationDbContext db;

        public HardwareUserControllerApi()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        ////Post
        //[HttpPost]
        //[Route("api/hardwareUserRequest/SaveRequest")]
        //public async Task<string> SaveUpload3()
        //{
        //    var ctx = HttpContext.Current;
        //    var root = ctx.Server.MapPath("~/HardwareImage/");


        //    int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
        //    int divId = Convert.ToInt32(User.Identity.GetUserDivision());

        //    var depName = db.Departments.SingleOrDefault(d => d.Id == depId).Name;
        //    var divName = db.Divisions.SingleOrDefault(d => d.Id == divId).Name;
        //    Random rand = new Random();
        //    int codeNum = rand.Next(00000, 99999);

        //    var provider = new MultipartFormDataStreamProvider(root);
        //    var ftpAddress = "192.168.1.171";
        //    var ftpUserName = "shielamaemalaque2022@outlook.com";
        //    var ftpPassword = "Malaque@22+08";

        //    FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
        //    ftpClient.Port = 21;
        //    ftpClient.Connect();
        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        HardwareUserRequest hardwareR = new HardwareUserRequest();

        //        if (hardwareR.Id == 0)
        //        {
        //            hardwareR.DocumentLabel = provider.FormData["FileName"];
        //            hardwareR.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
        //            hardwareR.DateCreated = DateTime.Now;
        //            hardwareR.FullName = User.Identity.GetFullName();
        //            hardwareR.Email = User.Identity.GetUserEmail();
        //            hardwareR.MobileNumber = User.Identity.GetUserMobileNumber();
        //            hardwareR.DepartmentsId = depId;
        //            hardwareR.DivisionsId = divId;
        //            hardwareR.DepartmentName = depName;
        //            hardwareR.DivisionName = divName;
        //            hardwareR.Description = provider.FormData["description"];
        //            hardwareR.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
        //            hardwareR.BrandName = provider.FormData["brandName"];
        //            hardwareR.ModelName = provider.FormData["modelName"];
        //            hardwareR.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
        //            hardwareR.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
        //            db.HardwareUserRequests.Add(hardwareR);
        //        }

        //        db.SaveChanges();

        //        foreach (var file in provider.FileData)
        //        {
        //            foreach (var key in provider.FormData.AllKeys)
        //            {
        //                foreach (var val in provider.FormData.GetValues(key))
        //                {

        //                    if (key == "FileName")
        //                    {
        //                        var name = file.Headers.ContentDisposition.FileName;

        //                        // remove double quotes from string.
        //                        var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";
        //                        name = name.Trim('"');

        //                        var localFileName = file.LocalFileName;
        //                        var filePath = Path.Combine(root, dateNew + name);

        //                        ftpClient.UploadFile(localFileName, "/HardwareImage/" + dateNew + name);

        //                        File.Delete(localFileName);

        //                        HardwareUserUploads hardwareuserUploads = new HardwareUserUploads();
        //                        hardwareuserUploads.FileName = name;
        //                        hardwareuserUploads.ImagePath = "/HardwareImage/" + dateNew + name;
        //                        hardwareuserUploads.HardwareUserRequestId = hardwareR.Id;
        //                        hardwareuserUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
        //                        db.HardwareUserUploads.Add(hardwareuserUploads);

        //                        db.SaveChanges();
        //                    }

        //                }
        //            }
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        return $"Error: {e.Message}";
        //    }
        //    ftpClient.Disconnect();
        //    return "File uploaded!";
        //}
    }
}
