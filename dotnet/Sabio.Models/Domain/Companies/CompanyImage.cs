using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Companies
{
    public class CompanyImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
    }

    public enum EntityType
    {
        SEO = 1,
        Cover = 2,
        Main = 3,
        Other = 4,
        Logo = 5,
    }
}
