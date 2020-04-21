using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Models
{
    public class MillWoodType
    {
        public int MillWoodTypeId { get; set; }
        [ForeignKey("Mill")]
        public int MillId { get; set; }
        public Mill Mill { get; set; }
        [ForeignKey("WoodType")]
        public int WoodTypeId { get; set; }
        public WoodType WoodType { get; set; }
    }
}
