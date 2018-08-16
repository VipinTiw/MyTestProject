using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EngJobMgmt.ApiService.Models;
using EngJobMgmt.Models;

namespace EngJobMgmt.ApiService.Controllers
{
    public class JobsController : ApiController
    {
        private EngJobMgmtApiServiceContext db = new EngJobMgmtApiServiceContext();

        // GET: api/Jobs
        public IQueryable<Job> GetJobs()
        {
            return db.Jobs;
        }

        // GET: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult GetJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // PUT: api/Jobs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.JobId)
            {
                return BadRequest();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Jobs
        [ResponseType(typeof(Job))]
        [HttpPost]
        public IHttpActionResult CreateJobRequest(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jobs.Add(job);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = job.JobId }, job);
        }

        public  IHttpActionResult AllocateJobs(int jobId, string Engineerid)
        {
            return Ok();
        }

        public IEnumerable<Job> GetJobList(string EngineerId)
        {
            return db.Jobs.Where(a=>a.EngineerId== EngineerId);
        }

        [HttpPut]
        public IHttpActionResult AcceptJob(int JobId)
        {
            var job = db.Jobs.Where(a => a.JobId == JobId).FirstOrDefault<Job>();
            if(job != null)
            {
                job.Jobstatus = "Accepted";

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }


        [HttpPut]
        public IHttpActionResult UpdateStatus(int JobId, string status)
        {
            var job = db.JobActivity.Where(a => a.JobId == JobId).FirstOrDefault<JobActivity>();
            if (job != null)
            {
                if (status == JobStatus.Completed.ToString())
                {
                    job.JobStatus = JobStatus.Completed;
                }
                else if (status == JobStatus.OnHold.ToString())
                {
                    job.JobStatus = JobStatus.OnHold;
                }
                else if (status == JobStatus.Onsite.ToString())
                {
                    job.JobStatus = JobStatus.Onsite;
                }

                db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        public IHttpActionResult MapSuperVisors(string SuperVisorId, List<Engineer> Engineers)
        {
            foreach (var item  in Engineers)
            {
                var Engineerdata = db.Engineer.Where(a => a.EngineerId == item.EngineerId).FirstOrDefault<Engineer>();
                if(Engineerdata != null)
                {
                    Engineerdata.SuperVisorId = SuperVisorId;
                }
            }
            db.SaveChanges();
            return Ok();
        }

        // DELETE: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult DeleteJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            db.SaveChanges();

            return Ok(job);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.JobId == id) > 0;
        }
    }
}