using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class AnalysisCreateModel
    {
        public Int32 LabId { get; set; }

        [Required]
        [MaxLength(200)]
        public String Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public String Status { get; set; }
    }
}
