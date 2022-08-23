using Sabio.Data.Providers;
using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Sabio.Data;
using System.Threading.Tasks;
using Sabio.Models.Requests.Friends;
using Sabio.Services.Interfaces;
using Sabio.Models;
using Newtonsoft.Json;

namespace Sabio.Services
{
    public class FriendService : IFriendService
    {
        private IDataProvider _data = null;

        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        #region Service version 1
        public int Add(FriendAddRequest addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Friends_Insert]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(addRequest, paramCollection, userId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                paramCollection.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@id"].Value;

                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Update(FriendUpdateRequest updateRequest, int userId)
        {
            string proc = "[dbo].[Friends_Update]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(updateRequest, paramCollection, userId);
                paramCollection.AddWithValue("@Id", updateRequest.Id);

            }, returnParameters: null);

        }

        public Friend Get(int id)
        {
            string proc = "[dbo].[Friends_SelectById]";
            Friend friend = null;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                friend = MapSingleFriend(reader, ref index);
            });

            return friend;
        }

        public List<Friend> GetAll()
        {
            string proc = "[dbo].[Friends_SelectAll]";
            List<Friend> friends = null;

            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Friend friend = MapSingleFriend(reader, ref index);

                if (friends == null)
                {
                    friends = new List<Friend>();
                }

                friends.Add(friend);
            });

            return friends;
        }

        public void Delete(int id)
        {
            string proc = "[dbo].[Friends_Delete]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }

        private static Friend MapSingleFriend(IDataReader reader, ref int startingIndex)
        {
            Friend friend = new Friend();
            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);
            return friend;
        }

        private static void AddCommonParams(FriendAddRequest addRequest, SqlParameterCollection paramCollection, int userId)
        {
            paramCollection.AddWithValue("@Title", addRequest.Title);
            paramCollection.AddWithValue("@Bio", addRequest.Bio);
            paramCollection.AddWithValue("@Summary", addRequest.Summary);
            paramCollection.AddWithValue("@Headline", addRequest.Headline);
            paramCollection.AddWithValue("@Slug", addRequest.Slug);
            paramCollection.AddWithValue("@StatusId", addRequest.StatusId);
            paramCollection.AddWithValue("@PrimaryImageUrl", addRequest.PrimaryImageUrl);
            paramCollection.AddWithValue("@UserId", userId);
        }
        #endregion

        #region Service version 2

        public int AddV2(FriendAddRequestV2 addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Friends_InsertV2]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParamsV2(addRequest, paramCollection, userId);

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

        public void UpdateV2(FriendUpdateRequestV2 updateReq, int userId)
        {
            string proc = "[dbo].[Friends_UpdateV2]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParamsV2(updateReq, paramCollection, userId);
                paramCollection.AddWithValue("@Id", updateReq.Id);

            }, returnParameters: null);
        }

        public FriendV2 GetV2(int id)
        {
            FriendV2 friend = null;
            string proc = "[dbo].[Friends_SelectByIdV2]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                friend = MapSingleFriendV2(reader, ref index);

            });


            return friend;
        }

        public List<FriendV2> GetAllV2()
        {
            List<FriendV2> friends = null;
            string proc = "[dbo].[Friends_SelectAllV2]";

            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                if (friends == null)
                {
                    friends = new List<FriendV2>();
                }

                FriendV2 friend = MapSingleFriendV2(reader, ref index);

                friends.Add(friend);
            });

            return friends;
        }

        public void DeleteV2(int id)
        {
            string proc = "[dbo].[Friends_DeleteV2]";
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }

        public Paged<FriendV2> PaginationV2(int pageIndex, int pageSize)
        {
            string proc = "[dbo].[Friends_PaginationV2]";
            List<FriendV2> friends = null;
            Paged<FriendV2> pagedItems = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonPageParam(pageIndex, pageSize, paramCollection);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                if (friends == null)
                {
                    friends = new List<FriendV2>();
                }

                FriendV2 friend = MapSingleFriendV2(reader, ref index);

                friends.Add(friend);

                total = reader.GetSafeInt32(index);

            });

            if(friends != null) {
                pagedItems = new Paged<FriendV2>(friends, pageIndex, pageSize, total);
            }
            

            return pagedItems;
        }


        public Paged<FriendV2> SearchV2(int pageIndex, int pageSize, string query)
        {
            string proc = "[dbo].[Friends_Search_PaginationV2]";
            List<FriendV2> friends = null;
            Paged<FriendV2> pagedItems = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonPageParam(pageIndex, pageSize, paramCollection);
                paramCollection.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                FriendV2 friend = MapSingleFriendV2(reader, ref index);

                if (friends == null)
                {
                    friends = new List<FriendV2>();
                }

                friends.Add(friend);

                total = reader.GetSafeInt32(index);
            });

            if (friends != null)
            {
                pagedItems = new Paged<FriendV2>(friends, pageIndex, pageSize, total);
            }

            return pagedItems;
        }

        private static FriendV2 MapSingleFriendV2(IDataReader reader, ref int startingIndex)
        {
            FriendV2 friend = new FriendV2();
            Image primaryImage = new Image();

            friend.Id = reader.GetSafeInt32(startingIndex++);
            primaryImage.Url = reader.GetSafeString(startingIndex++);
            primaryImage.Id = reader.GetSafeInt32(startingIndex++);
            primaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.DateCreated = reader.GetDateTime(startingIndex++);
            friend.DateModified = reader.GetDateTime(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);

            friend.PrimaryImage = primaryImage;

            return friend;
        }

        private static void AddCommonParamsV2(FriendAddRequestV2 addRequest, SqlParameterCollection paramCollection, int userId)
        {
            paramCollection.AddWithValue("@Title", addRequest.Title);
            paramCollection.AddWithValue("@Bio", addRequest.Bio);
            paramCollection.AddWithValue("@Summary", addRequest.Summary);
            paramCollection.AddWithValue("@Headline", addRequest.Headline);
            paramCollection.AddWithValue("@Slug", addRequest.Slug);
            paramCollection.AddWithValue("@StatusId", addRequest.StatusId);
            paramCollection.AddWithValue("@ImageTypeId", addRequest.ImageTypeId);
            paramCollection.AddWithValue("@ImageUrl", addRequest.ImageUrl);
            paramCollection.AddWithValue("@UserId", userId);
        }
        #endregion

        #region Service version 3
        public int AddV3(FriendAddRequestV3 addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Friends_InsertV3]";
            DataTable skillsBatch = null; 

            if (addRequest.Skills != null)
            {
                skillsBatch = MapSkillsToTable(addRequest.Skills);  
            }

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                AddCommonParamsV3(addRequest, parameterCol, userId);
                parameterCol.AddWithValue("@skillsBatch", skillsBatch);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                parameterCol.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });


            return id;
        }

        public void UpdateV3(FriendUpdateRequestV3 updateReq, int userId)
        {
            string proc = "[dbo].[Friends_UpdateV3]";
            DataTable skillsBatch = null;

            if (updateReq.Skills != null)
            {
                skillsBatch = MapSkillsToTable(updateReq.Skills);
            }

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                AddCommonParamsV3(updateReq, parameterCol, userId);
                parameterCol.AddWithValue("@skillsBatch", skillsBatch);
                parameterCol.AddWithValue("@Id", updateReq.Id);

            }, returnParameters: null);

        }

        public FriendV3 GetV3(int id)
        {
            FriendV3 friend = null;
            string proc = "[dbo].[Friends_SelectByIdV3]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                friend = MapSingleFriendV3(reader, ref index);

            });


            return friend;
        }

        public List<FriendV3> GetAllV3()
        {
            List<FriendV3> friends = null;
            string proc = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                FriendV3 friend = MapSingleFriendV3(reader, ref index);

                if (friends == null)
                {
                    friends = new List<FriendV3>();
                }

                friends.Add(friend);
            });

            return friends;
        }

        public void DeleteV3(int id)
        {
            string proc = "[dbo].[Friends_DeleteV3]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            });
        }

        public Paged<FriendV3> PaginationV3(int pageIndex, int pageSize)
        {
            Paged<FriendV3> pagedFriends = null;
            int total = 0;
            List<FriendV3> friendList = null;
            string proc = "[dbo].[Friends_PaginationV3]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonPageParam(pageIndex, pageSize, paramCollection);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                FriendV3 friend = MapSingleFriendV3(reader, ref index);

                if (friendList == null)
                {
                    friendList = new List<FriendV3>();
                }

                friendList.Add(friend);
                total = reader.GetSafeInt32(index);

            });
            if(friendList != null) {
                pagedFriends = new Paged<FriendV3>(friendList, pageIndex, pageSize, total);
            }
            

            return pagedFriends;
        }

        public Paged<FriendV3> SearchV3(int pageIndex, int pageSize, string query)
        {
            Paged<FriendV3> pagedFriends = null;
            int total = 0;
            List<FriendV3> friendList = null;
            string proc = "[dbo].[Friends_Search_PaginationV3]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonPageParam(pageIndex, pageSize, paramCollection);
                paramCollection.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                FriendV3 friend = MapSingleFriendV3(reader, ref index);

                if (friendList == null)
                {
                    friendList = new List<FriendV3>();
                }

                friendList.Add(friend);

                total = reader.GetSafeInt32(index);

            });

            if (friendList != null)
            {
                pagedFriends = new Paged<FriendV3>(friendList, pageIndex, pageSize, total);
            }

            return pagedFriends;
        }

        private static FriendV3 MapSingleFriendV3(IDataReader reader, ref int index)
        {
            FriendV3 friend = new FriendV3();
            Image primaryImage = new Image();
            Skill skill = new Skill();

            friend.Id = reader.GetSafeInt32(index++);
            primaryImage.Url = reader.GetSafeString(index++);
            primaryImage.Id = reader.GetSafeInt32(index++);
            primaryImage.TypeId = reader.GetSafeInt32(index++);
            friend.Title = reader.GetSafeString(index++);
            friend.Bio = reader.GetSafeString(index++);
            friend.Summary = reader.GetSafeString(index++);
            friend.Headline = reader.GetSafeString(index++);
            friend.Slug = reader.GetSafeString(index++);
            friend.StatusId = reader.GetSafeInt32(index++);
            friend.DateCreated = reader.GetSafeDateTime(index++);
            friend.DateModified = reader.GetSafeDateTime(index++);
            friend.UserId = reader.GetSafeInt32(index++);

            string list = reader.GetSafeString(index++);

            if (!string.IsNullOrEmpty(list))
            {
                friend.Skills = JsonConvert.DeserializeObject<List<Skill>>(list);
            }
            /* Another option, object provided by Sabio:
              friend.Skills = reader.DeserializeObject<List<Skill>>(list);*/

            friend.PrimaryImage = primaryImage;
            return friend;
        }

        private static void AddCommonParamsV3(FriendAddRequestV3 request, SqlParameterCollection parameterCol, int userId)
        {
            parameterCol.AddWithValue("@Title", request.Title);
            parameterCol.AddWithValue("@Bio", request.Bio);
            parameterCol.AddWithValue("@Summary", request.Summary);
            parameterCol.AddWithValue("@Headline", request.Headline);
            parameterCol.AddWithValue("@Slug", request.Slug);
            parameterCol.AddWithValue("@StatusId", request.StatusId);
            parameterCol.AddWithValue("@ImageTypeId", request.ImageTypeId);
            parameterCol.AddWithValue("@ImageUrl", request.ImageUrl);
            parameterCol.AddWithValue("@UserId", userId);
            
        }

        private static DataTable MapSkillsToTable(List<string> Skills)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            foreach (string skill in Skills)
            {
                DataRow dr = dt.NewRow();
                int startingIndex = 0;

                dr.SetField(startingIndex++, skill);

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        private static void AddCommonPageParam(int pageIndex, int pageSize, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@PageIndex", pageIndex);
            paramCollection.AddWithValue("@PageSize", pageSize);
        }


    }
}
