using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendAddRequestV2
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(700, MinimumLength = 2)]
        public string Bio { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Summary { get; set; }
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Headline { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int StatusId { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int ImageTypeId { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
