using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Events
{
    public class EventAddRequest
    {
        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Headline { get; set; }
        [Required]
        [StringLength(Int32.MaxValue, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Summary { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required]
        public string StatusId { get; set; }
        [Required]
        public MetaDataAddRequest Metadata { get; set; }
    }
}
