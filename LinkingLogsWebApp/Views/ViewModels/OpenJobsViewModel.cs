using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Views.ViewModels
{
    public class OpenJobsViewModel
    {
        public IEnumerable<Site> Sites { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
        public SiteManager SiteManager { get; set; }
    }
}
