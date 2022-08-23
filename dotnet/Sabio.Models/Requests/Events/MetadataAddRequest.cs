using Sabio.Models.Domain.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Events
{
    public class MetaDataAddRequest
    {
        [Required]
        public string DateStart { get; set; }
        [Required]
        public string DateEnd { get; set; }
        [Required]
        public LocationAddRequest Location { get; set; }
    }
}
