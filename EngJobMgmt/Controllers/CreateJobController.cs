using EngJobMgmt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngJobMgmt.Controllers
{
    public class CreateJobController : Controller
    {

        // GET: CreateJob
        [HttpGet]
        public ActionResult CreateJob()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult CreateJob(Job newJob)
        //{

        //}
    }
}