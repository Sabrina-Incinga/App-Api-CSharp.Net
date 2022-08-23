using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Companies
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string ContactInformation { get; set; }
        public string Slug { get; set; }
        public string StatusId { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Url> Urls { get; set; }
        public List<CompanyImage> Images { get; set; }
        public List<FriendV3> Friends { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }
    }
}
