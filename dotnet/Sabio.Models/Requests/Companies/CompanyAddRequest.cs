using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Companies
{
    public class CompanyAddRequest
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(Int32.MaxValue, MinimumLength = 2)]
        public string Profile { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Summary { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Headline { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string ContactInformation { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string StatusId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Urls { get; set; }
        public List<CoImageAddRequest> Images { get; set; }
        public List<int> FriendIds { get; set; }
    }
}
