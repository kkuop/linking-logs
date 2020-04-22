using LinkingLogsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Views.ViewModels
{
    public class ApproveBidModel
    {
        public JobBid JobBid { get; set; }
        public Trucker Trucker { get; set; }
    }
}
