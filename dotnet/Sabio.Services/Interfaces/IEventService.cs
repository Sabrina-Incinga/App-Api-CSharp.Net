using Sabio.Models;
using Sabio.Models.Domain.Events;
using Sabio.Models.Requests.Events;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IEventService
    {
        public int Add(EventAddRequest addRequest, int userId);
        public void Update(EventUpdateRequest updateRequest, int userId);
        Event Get(int id);
        public List<Event> GetAllUpcoming();
        public Paged<Event> SearchByDate(int pageIndex, int pageSize, string dateStart, string dateEnd);
        public Paged<Event> Pagination(int pageIndex, int pageSize);
        public Event GetBySlug(string slug);
        public Paged<Event> SearchByGeo(int pageIndex, int pageSize, int radius, decimal startingLatitude, decimal startingLongitude);
    }
}