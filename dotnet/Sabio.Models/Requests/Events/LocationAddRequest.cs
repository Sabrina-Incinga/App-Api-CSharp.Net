using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Events
{
    public class LocationAddRequest
    {
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string ZipCode { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
