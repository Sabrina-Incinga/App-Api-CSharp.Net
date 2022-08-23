using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Jobs
{
    public class JobAddRequest
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(Int32.MaxValue, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Summary { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Pay { get; set; }
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }
        [StringLength(10, MinimumLength = 2)]
        public string StatusId { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int TechCompanyId { get; set; }
        public List<string> Skills { get; set; }
    }
}
