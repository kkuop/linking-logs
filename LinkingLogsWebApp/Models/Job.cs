using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Models
{
    public class Job
    {
        public int JobId { get; set; }
        [Display(Name = "Load Size")]
        public int LoadSize { get; set; }
        public string Status { get; set; }
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }
        [ForeignKey("WoodType")]
        public int WoodTypeId { get; set; }
        public WoodType WoodType { get; set; }
        [ForeignKey("Mill")]
        public int MillId { get; set; }
        public Mill Mill { get; set; }
        [NotMapped]
        public IEnumerable<WoodType> WoodTypes { get; set; }
        [NotMapped]
        public IEnumerable<Site> Sites { get; set; }
        [NotMapped]
        public IEnumerable<Mill> Mills { get; set; }
        [NotMapped]
        public int Loads { get; set; }
        [NotMapped]
        public double Distance { get; set; }
    }
}
