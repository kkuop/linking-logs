using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Views.ViewModels
{
    public class TruckerIndexViewModel
    {
        public IEnumerable<Job> AllJobs { get; set; }
        public IEnumerable<Job> SuggestedJobs { get; set; }
        public IEnumerable<Job> PendingJobs { get; set; }
        public IEnumerable<Job> JobsWon { get; set; }
        public IEnumerable<JobBid> PendingBids { get; set; } 
        public Trucker Trucker { get; set; }
    }
}
