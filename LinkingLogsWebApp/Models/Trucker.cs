using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Models
{
    public class Trucker
    {
        public int TruckerId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
