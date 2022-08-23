using Sabio.Data.Providers;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Sabio.Data;
using System.Threading.Tasks;
using Sabio.Models.Requests.Users;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1
    {
        private IDataProvider _data = null;
        public UserServiceV1(IDataProvider data)
        {
            _data = data;
        }

        public int Add(UserAddRequest addRequest)
        {
            string proc = "[dbo].[Users_Insert]";
            int id = 0;

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(addRequest, paramCollection);

                //Get id output:
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                paramCollection.Add(idOut);
            },
                returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object oId = returnCol["@id"].Value;

                    int.TryParse(oId.ToString(), out id);
                }
                );

            return id;
        }

        public void Update(UserUpdateRequest updateRequest)
        {
            string proc = "[dbo].[Users_Update]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(updateRequest, paramCollection);
                paramCollection.AddWithValue("@Id", updateRequest.Id);
            },
                returnParameters: null
                );
        }

        public User Get(int id)
        {
            User user = null;
            string proc = "[dbo].[Users_SelectById]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                parameterCol.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                user = MapSingleUser(reader);
            });

            return user;
        }

        public List<User> GetAll()
        {
            List<User> userList = null;
            string proc = "[dbo].[Users_SelectAll]";

            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User user = MapSingleUser(reader);

                if (userList == null)
                {
                    userList = new List<User>();
                }
                userList.Add(user);

            });

            return userList;
        }

        public void Delete(int id)
        {
            string proc = "[dbo].[Users_Delete]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                parameterCol.AddWithValue("@Id", id);
            }, returnParameters: null);

        }

        private static User MapSingleUser(IDataReader reader)
        {
            int startingIndex = 0;
            User user = new User();

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.TenantId = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            return user;
        }

        private static void AddCommonParams(UserAddRequest addRequest, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("FirstName", addRequest.FirstName);
            paramCollection.AddWithValue("LastName", addRequest.LastName);
            paramCollection.AddWithValue("Email", addRequest.Email);
            paramCollection.AddWithValue("AvatarUrl", addRequest.AvatarUrl);
            paramCollection.AddWithValue("TenantId", addRequest.TenantId);
            paramCollection.AddWithValue("Password", addRequest.Password);
        }
    }
}
