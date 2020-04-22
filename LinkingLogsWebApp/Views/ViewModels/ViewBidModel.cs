using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Views.ViewModels
{
    public class ViewBidModel
    {
        public Job Job { get; set; }
        public List<JobBid> JobBids { get; set; }
        public List<Trucker> Truckers { get; set; }
    }
}
