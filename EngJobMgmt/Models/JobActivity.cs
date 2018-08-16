using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngJobMgmt.Models
{
    public class JobActivity
    {
        public int JobActivityId { get; set; }

        public int JobId { get; set; }

        public JobStatus JobStatus { get; set; }

        public string  Comment { get; set; }
    }

    public enum JobStatus
    {
        Onsite,
        Completed,
        OnHold
    }
}