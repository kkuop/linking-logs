using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Models
{
    public class JobBid
    {
        public int JobBidId { get; set; }
        public bool IsWinningBid { get; set; }
        public double AmountBid { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }
        [ForeignKey("Trucker")]
        public int TruckerId { get; set; }
        public Trucker Trucker { get; set; }
    }
}
