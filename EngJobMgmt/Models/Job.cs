using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EngJobMgmt.Models
{
    public class Job
    {

        public int JobId { get; set; }

        public string CustomerName { get; set; }

        public string  Address { get; set; }

        public decimal Telephone { get; set; }


        public JobItem  JobItem { get; set; }

        public JobType JobType { get; set; }

        public string  Description { get; set; }

        public DateTime PreferedeDateTime { get; set; }

        public string  Jobstatus { get; set; }

        public string EngineerId { get; set; }
    }

    public enum JobItem
    {
        Broadband,
        TV
    }

    public enum JobType
    {
        Install ,
        Repair
    }
}