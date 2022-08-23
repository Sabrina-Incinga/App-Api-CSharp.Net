using Sabio.Data.Providers;
using Sabio.Models.Domain.Events;
using System;
using System.Collections.Generic;
using System.Data;
using Sabio.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Services.Interfaces;
using Sabio.Models;
using Sabio.Models.Requests.Events;

namespace Sabio.Services
{
    public class EventService : IEventService
    {
        private IDataProvider _data = null;

        public EventService(IDataProvider data)
        {
            _data = data;
        }
        public int Add(EventAddRequest addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Events_Insert]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(addRequest, userId, paramCollection);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                paramCollection.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Update(EventUpdateRequest updateRequest, int userId)
        {
            string proc = "[dbo].[Events_Update]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(updateRequest, userId, paramCollection);
                paramCollection.AddWithValue("@Id", updateRequest.Id);

            }, returnParameters: null);
        }

        public Event Get(int id)
        {
            Event anEvent = null;
            string proc = "[dbo].[Events_SelectById]";
            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                anEvent = MapSingleEvent(reader, ref index);

            });

            return anEvent;
        }

        public Event GetBySlug(string slug)
        {
            Event anEvent = null;
            string proc = "[dbo].[Events_SelectBySlug]";
            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@slug", slug);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                anEvent = MapSingleEvent(reader, ref index);

            });

            return anEvent;
        }

        public List<Event> GetAllUpcoming()
        {
            List<Event> list = null;
            string proc = "[dbo].[Events_Feeds]";
            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Event anEvent = MapSingleEvent(reader, ref index);

                if(list == null)
                {
                    list = new List<Event>();
                }

                list.Add(anEvent);

            });

            return list;
        }

        public Paged<Event> SearchByDate(int pageIndex, int pageSize, string dateStart, string dateEnd)
        {
            List<Event> list = null;
            Paged<Event> page = null;
            int total = 0;
            string proc = "[dbo].[Events_Search_Pagination]";
            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@dateStart", dateStart);
                paramCollection.AddWithValue("@dateEnd", dateEnd);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Event anEvent = MapSingleEvent(reader, ref index);

                if (list == null)
                {
                    list = new List<Event>();
                }

                list.Add(anEvent);
                total = reader.GetSafeInt32(index);

            });
            if(list != null) {
                page = new Paged<Event>(list, pageIndex, pageSize, total);
            }
            

            return page;
        }

        public Paged<Event> SearchByGeo(int pageIndex, int pageSize, int radius, decimal startingLatitude, decimal startingLongitude)
        {
            List<Event> list = null;
            Paged<Event> page = null;
            int total = 0;
            string proc = "[dbo].[Events_SearchByGeo_Pagination]";
            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@radius", radius);
                paramCollection.AddWithValue("@startingLatitude", startingLatitude);
                paramCollection.AddWithValue("@startingLongitude", startingLongitude);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Event anEvent = MapSingleEvent(reader, ref index);

                if (list == null)
                {
                    list = new List<Event>();
                }

                list.Add(anEvent);
                total = reader.GetSafeInt32(index);

            });

            if (list != null)
            {
                page = new Paged<Event>(list, pageIndex, pageSize, total);
            }

            return page;
        }

        public Paged<Event> Pagination(int pageIndex, int pageSize)
        {
            List<Event> list = null;
            Paged<Event> page = null;
            string proc = "[dbo].[Events_Pagination]";
            int total = 0;
            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Event anEvent = MapSingleEvent(reader, ref index);

                if (list == null)
                {
                    list = new List<Event>();
                }

                list.Add(anEvent);

                total = reader.GetSafeInt32(index);

            });

            if (list != null)
            {
                page = new Paged<Event>(list, pageIndex, pageSize, total);
            }

            return page;
        }

        private static Event MapSingleEvent(IDataReader reader, ref int index)
        {
            Event anEvent = new Event();
            MetaData aMetaData = new MetaData();
            Location aLocation = new Location();

            anEvent.Id = reader.GetSafeInt32(index++);
            anEvent.Name = reader.GetSafeString(index++);
            anEvent.Headline = reader.GetSafeString(index++);
            anEvent.Description = reader.GetSafeString(index++);
            anEvent.Summary = reader.GetSafeString(index++);
            anEvent.Slug = reader.GetSafeString(index++);
            anEvent.StatusId = reader.GetSafeString(index++);
            aMetaData.DateStart = reader.GetDateTime(index++);
            aMetaData.DateEnd = reader.GetDateTime(index++);
            aLocation.Latitude = reader.GetSafeDecimal(index++);
            aLocation.Longitude = reader.GetSafeDecimal(index++);
            aLocation.Address = reader.GetSafeString(index++);
            aLocation.ZipCode = reader.GetSafeString(index++);

            aMetaData.Location = aLocation;
            anEvent.Metadata = aMetaData;

            return anEvent;
        }

        private static void AddCommonParams(EventAddRequest addRequest, int userId, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@Name", addRequest.Name);
            paramCollection.AddWithValue("@Headline", addRequest.Headline);
            paramCollection.AddWithValue("@Description", addRequest.Description);
            paramCollection.AddWithValue("@Summary", addRequest.Summary);
            paramCollection.AddWithValue("@Slug", addRequest.Slug);
            paramCollection.AddWithValue("@StatusId", addRequest.StatusId);
            paramCollection.AddWithValue("@DateStart", addRequest.Metadata.DateStart);
            paramCollection.AddWithValue("@DateEnd", addRequest.Metadata.DateEnd);
            paramCollection.AddWithValue("@Latitude", addRequest.Metadata.Location.Latitude);
            paramCollection.AddWithValue("@Longitude", addRequest.Metadata.Location.Longitude);
            paramCollection.AddWithValue("@ZipCode", addRequest.Metadata.Location.ZipCode);
            paramCollection.AddWithValue("@Address", addRequest.Metadata.Location.Address);
            paramCollection.AddWithValue("@UserId", userId);
        }

    }
}
