using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Views.ViewModels
{
    public class SiteManagerIndexViewModel
    {
        public IEnumerable<Job> AllJobs { get; set; }
        public IEnumerable<Job> OpenJobs { get; set; }
        public IEnumerable<Job> ApprovedJobs { get; set; }
        public IEnumerable<Job> PendingJobs { get; set; }
        public IEnumerable<Job> CompletedJobs { get; set; }
        public IEnumerable<Site> AllSites { get; set; }
        public IEnumerable<Site> UpcomingSites { get; set; }
        public IEnumerable<Site> ActiveSites { get; set; }
        public IEnumerable<Site> ClosedSites { get; set; }
    }
}
