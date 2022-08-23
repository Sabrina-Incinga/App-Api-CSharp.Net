using System;

namespace Sabio.Models.Domain.Events
{
    public class MetaData
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Location Location { get; set; }

    }
}