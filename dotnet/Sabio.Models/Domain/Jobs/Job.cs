using Sabio.Models.Domain.Companies;
using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Jobs
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Pay { get; set; }
        public string Slug { get; set; }
        public string StatusId { get; set; }
        public Company TechCompany { get; set; }
        public List<Skill> Skills { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }
    }
}
