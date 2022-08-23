using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Companies
{
    public class CoImageAddRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string ImageUrl { get; set; }
        [Required]
        public int EntityTypeId { get; set; }
    }

}
