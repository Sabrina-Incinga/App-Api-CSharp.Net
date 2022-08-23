﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Friends
{
    public class Friend : FriendBasic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        public int StatusId { get; set; }
        public string PrimaryImageUrl { get; set; }
    }
}
