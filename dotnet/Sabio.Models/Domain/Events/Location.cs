namespace Sabio.Models.Domain.Events
{
    public class Location 
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }

    }
}