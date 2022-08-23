using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Friends
{
    public class FriendBasic
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }

    }
}
