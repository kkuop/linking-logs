using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Display(Name="Opening Date")]
        public DateTime OpeningDate { get; set; }
        [Display(Name="Closing Date")]
        public DateTime ClosingDate { get; set; }
        [ForeignKey("SiteManager")]
        public int SiteManagerId { get; set; }
        public SiteManager SiteManager { get; set; }
    }
}
