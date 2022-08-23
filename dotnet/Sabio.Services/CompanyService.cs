using Sabio.Data.Providers;
using Sabio.Models.Domain.Companies;
using System;
using System.Collections.Generic;
using System.Data;
using Sabio.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Friends;
using Sabio.Models;
using Sabio.Models.Requests.Companies;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class CompanyService : ICompanyService
    {
        private IDataProvider _data = null;

        public CompanyService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(CompanyAddRequest addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Companies_Insert]";
            DataTable imagesBatch = null;
            DataTable urlsBatch = null;
            DataTable tagsBatch = null;
            DataTable friendIdsBatch = null;

            if (addRequest.Images != null)
            {
                imagesBatch = MapImagesToTable(addRequest.Images);
            }
            if (addRequest.Urls != null)
            {
                urlsBatch = MapUrlsToTable(addRequest.Urls);
            }
            if (addRequest.Tags != null)
            {
                tagsBatch = MapTagsToTable(addRequest.Tags);
            }
            if (addRequest.FriendIds != null)
            {
                friendIdsBatch = MapFriendIdsToTable(addRequest.FriendIds);
            }

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                AddCommonParams(addRequest, userId, parameterCol, imagesBatch, urlsBatch, tagsBatch, friendIdsBatch);

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

        public void Update(CompanyUpdateRequest updateRequest, int userId)
        {
            string proc = "[dbo].[Companies_Update]";
            DataTable imagesBatch = null;
            DataTable urlsBatch = null;
            DataTable tagsBatch = null;
            DataTable friendIdsBatch = null;

            if (updateRequest.Images != null)
            {
                imagesBatch = MapImagesToTable(updateRequest.Images);
            }
            if (updateRequest.Urls != null)
            {
                urlsBatch = MapUrlsToTable(updateRequest.Urls);
            }
            if (updateRequest.Tags != null)
            {
                tagsBatch = MapTagsToTable(updateRequest.Tags);
            }
            if (updateRequest.FriendIds != null)
            {
                friendIdsBatch = MapFriendIdsToTable(updateRequest.FriendIds);
            }

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection parameterCol)
            {
                AddCommonParams(updateRequest, userId, parameterCol, imagesBatch, urlsBatch, tagsBatch, friendIdsBatch);
                parameterCol.AddWithValue("@Id", updateRequest.Id);

            }, returnParameters: null);

        }

        public Company Get(int id)
        {
            Company company = null;
            string proc = "[dbo].[Companies_SelectById]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                company = MapSingleCompany(reader, ref index);

            });

            return company;
        }

        public Company GetBySlug(string slug)
        {
            Company company = null;
            string proc = "[dbo].[Companies_SelectBySlug]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Slug", slug);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                company = MapSingleCompany(reader, ref index);

            });

            return company;
        }

        public List<Company> GetAll()
        {
            List<Company> companies = null;
            string proc = "[dbo].[Companies_SelectAll]";
            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Company company = MapSingleCompany(reader, ref index);

                if (companies == null)
                {
                    companies = new List<Company>();
                }

                companies.Add(company);
            });


            return companies;
        }

        public Paged<Company> Pagination(int pageIndex, int pageSize)
        {
            string proc = "[dbo].[Companies_Pagination]";
            List<Company> companies = null;
            Paged<Company> page = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonSearchParams(pageIndex, pageSize, paramCollection);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Company company = MapSingleCompany(reader, ref index);

                if (companies == null)
                {
                    companies = new List<Company>();
                }

                companies.Add(company);
                total = reader.GetSafeInt32(index);

            });

            if(companies != null){ 
                page = new Paged<Company>(companies, pageIndex, pageSize, total); 
            }

            return page;
        }

        public Paged<Company> Search(int pageIndex, int pageSize, string query)
        {
            string proc = "[dbo].[Companies_Search]";
            List<Company> companies = null;
            Paged<Company> page = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonSearchParams(pageIndex, pageSize, paramCollection);
                paramCollection.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Company company = MapSingleCompany(reader, ref index);

                if (companies == null)
                {
                    companies = new List<Company>();
                }

                companies.Add(company);
                total = reader.GetSafeInt32(index);

            });

            if (companies != null)
            {
                page = new Paged<Company>(companies, pageIndex, pageSize, total);
            }

            return page;
        }

        public void UpdateStatusId(int id, string statusId, int userId)
        {
            string proc = "[dbo].[Companies_UpdateStatusId]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
                paramCollection.AddWithValue("@StatusId", statusId);
                paramCollection.AddWithValue("@UserId", userId);
            });

        }

        private static Company MapSingleCompany(IDataReader reader, ref int index)
        {
            Company company = new Company();

            company.Id = reader.GetSafeInt32(index++);
            company.Name = reader.GetString(index++);
            company.Profile = reader.GetString(index++);
            company.Summary = reader.GetString(index++);
            company.Headline = reader.GetString(index++);
            company.ContactInformation = reader.GetString(index++);
            company.Slug = reader.GetString(index++);
            company.StatusId = reader.GetString(index++);
            company.UserId = reader.GetSafeInt32(index++);
            company.DateCreated = reader.GetSafeDateTime(index++);
            company.DateModified = reader.GetSafeDateTime(index++);
            company.Urls = reader.DeserializeObject<List<Url>>(index++);
            company.Images = reader.DeserializeObject<List<CompanyImage>>(index++);
            company.Tags = reader.DeserializeObject<List<Tag>>(index++);
            company.Friends = reader.DeserializeObject<List<FriendV3>>(index++);

            List<Image> primaryImages = reader.DeserializeObject<List<Image>>(index++);

            if (primaryImages != null)
            {
                for (int i = 0; i < primaryImages.Count; i++)
                {
                    for (int j = 0; j < company.Friends.Count; j++)
                    {
                        if (j == i)
                        {
                            company.Friends[j].PrimaryImage = primaryImages[i];
                        }

                    }
                }
            }

            return company;
        }

        private static void AddCommonSearchParams(int pageIndex, int pageSize, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@PageIndex", pageIndex);
            paramCollection.AddWithValue("@PageSize", pageSize);
        }

        private static void AddCommonParams(CompanyAddRequest addRequest, int userId, SqlParameterCollection parameterCol, DataTable imagesBatch, DataTable urlsBatch, DataTable tagsBatch, DataTable friendIdsBatch)
        {
            parameterCol.AddWithValue("@Name", addRequest.Name);
            parameterCol.AddWithValue("@Profile", addRequest.Profile);
            parameterCol.AddWithValue("@Summary", addRequest.Summary);
            parameterCol.AddWithValue("@Headline", addRequest.Headline);
            parameterCol.AddWithValue("@ContactInformation", addRequest.ContactInformation);
            parameterCol.AddWithValue("@Slug", addRequest.Slug);
            parameterCol.AddWithValue("@StatusId", addRequest.StatusId);
            parameterCol.AddWithValue("@UserId", userId);
            parameterCol.AddWithValue("@ImagesBatch", imagesBatch);
            parameterCol.AddWithValue("@UrlsBatch", urlsBatch);
            parameterCol.AddWithValue("@TagsBatch", tagsBatch);
            parameterCol.AddWithValue("@FriendIdsBatch", friendIdsBatch);
        }

        private static DataTable MapImagesToTable(List<CoImageAddRequest> images)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("EntityTypeId", typeof(Int32));

            foreach (CoImageAddRequest image in images)
            {
                DataRow dr = dt.NewRow();
                int index = 0;

                dr.SetField(index++, image.ImageUrl);
                dr.SetField(index++, image.EntityTypeId);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static DataTable MapUrlsToTable(List<string> urls)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Url", typeof(string));
            foreach (string url in urls)
            {
                DataRow dr = dt.NewRow();
                int index = 0;

                dr.SetField(index++, url);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static DataTable MapTagsToTable(List<string> tags)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TagName", typeof(string));
            foreach (string tag in tags)
            {
                DataRow dr = dt.NewRow();
                int index = 0;

                dr.SetField(index++, tag);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static DataTable MapFriendIdsToTable(List<int> friendIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TagName", typeof(int));
            foreach (int friendId in friendIds)
            {
                DataRow dr = dt.NewRow();
                int index = 0;

                dr.SetField(index++, friendId);

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
