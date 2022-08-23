using Sabio.Data.Providers;
using Sabio.Models.Domain.Jobs;
using Sabio.Models.Requests.Jobs;
using System;
using System.Collections.Generic;
using System.Data;
using Sabio.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Companies;
using Sabio.Models.Domain.Friends;
using Sabio.Models;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class JobService : IJobService
    {
        private IDataProvider _data = null;

        public JobService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(JobAddRequest addRequest, int userId)
        {
            int id = 0;
            string proc = "[dbo].[Jobs_Insert_SkillsBatch]";
            DataTable skillsBatch = null;

            if (addRequest.Skills != null)
            {
                skillsBatch = MapSkillsToTable(addRequest.Skills);
            }
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(addRequest, userId, paramCollection, skillsBatch);

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

        public void Update(JobUpdateRequest updateRequest, int userId)
        {
            string proc = "[dbo].[Jobs_Update_SkillsBatch]";
            DataTable skillsBatch = null;

            if (updateRequest.Skills != null)
            {
                skillsBatch = MapSkillsToTable(updateRequest.Skills);
            }
            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonParams(updateRequest, userId, paramCollection, skillsBatch);
                paramCollection.AddWithValue("@Id", updateRequest.Id);

            }, returnParameters: null);

        }

        public Job Get(int id)
        {
            Job job = null;
            string proc = "[dbo].[Jobs_SelectById]";

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                job = MapSingleJob(reader, ref startingIndex);

            });

            return job;
        }

        public List<Job> GetAll()
        {
            List<Job> list = null;
            string proc = "[dbo].[Jobs_SelectAll]";

            _data.ExecuteCmd(proc, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Job job = MapSingleJob(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<Job>();
                }

                list.Add(job);

            });

            return list;
        }

        public Paged<Job> Pagination(int pageIndex, int pageSize)
        {
            Paged<Job> page = null;
            string proc = "[dbo].[Jobs_Pagination]";
            List<Job> list = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonSearchParams(pageIndex, pageSize, paramCollection);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Job job = MapSingleJob(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<Job>();
                }

                list.Add(job);
                total = reader.GetSafeInt32(startingIndex);

            });

            if(list != null)
            {
                page = new Paged<Job>(list, pageIndex, pageSize, total);
            }
            
            return page;
        }

        public Paged<Job> Search(int pageIndex, int pageSize, string query)
        {
            Paged<Job> page = null;
            string proc = "[dbo].[Jobs_Search_Pagination]";
            List<Job> list = null;
            int total = 0;

            _data.ExecuteCmd(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                AddCommonSearchParams(pageIndex, pageSize, paramCollection);
                paramCollection.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Job job = MapSingleJob(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<Job>();
                }

                list.Add(job);
                total = reader.GetSafeInt32(startingIndex);

            });

            if (list != null)
            {
                page = new Paged<Job>(list, pageIndex, pageSize, total);
            }

            return page;
        }

        public void Delete(int id)
        {
            string proc = "[dbo].[Jobs_Delete]";

            _data.ExecuteNonQuery(proc, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });

        }
        private static Job MapSingleJob(IDataReader reader, ref int startingIndex)
        {
            Job job = new Job();
            Company company = new Company();

            job.Id = reader.GetSafeInt32(startingIndex++);
            job.Title = reader.GetSafeString(startingIndex++);
            job.Description = reader.GetSafeString(startingIndex++);
            job.Summary = reader.GetSafeString(startingIndex++);
            job.Pay = reader.GetSafeString(startingIndex++);
            job.Slug = reader.GetSafeString(startingIndex++);
            job.StatusId = reader.GetSafeString(startingIndex++);
            job.DateCreated = reader.GetSafeDateTime(startingIndex++);
            job.DateModified = reader.GetSafeDateTime(startingIndex++);
            job.UserId = reader.GetSafeInt32(startingIndex++);
            company.Id = reader.GetSafeInt32(startingIndex++);
            company.Name = reader.GetSafeString(startingIndex++);
            company.Profile = reader.GetSafeString(startingIndex++);
            company.Summary = reader.GetSafeString(startingIndex++);
            company.Headline = reader.GetSafeString(startingIndex++);
            company.ContactInformation = reader.GetSafeString(startingIndex++);
            company.Slug = reader.GetSafeString(startingIndex++);
            company.StatusId = reader.GetSafeString(startingIndex++);
            company.DateCreated = reader.GetSafeDateTime(startingIndex++);
            company.DateModified = reader.GetSafeDateTime(startingIndex++);
            company.UserId = reader.GetSafeInt32(startingIndex++);
            company.Urls = reader.DeserializeObject<List<Url>>(startingIndex++);
            company.Images = reader.DeserializeObject<List<CompanyImage>>(startingIndex++);
            company.Tags = reader.DeserializeObject<List<Tag>>(startingIndex++);
            company.Friends = reader.DeserializeObject<List<FriendV3>>(startingIndex++);
            job.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);

            List<Image> primaryImages = reader.DeserializeObject<List<Image>>(startingIndex++);

            if (primaryImages != null && company.Friends != null)
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

            job.TechCompany = company;
            return job;
        }

        private static void AddCommonParams(JobAddRequest addRequest, int userId, SqlParameterCollection paramCollection, DataTable skillsBatch)
        {
            paramCollection.AddWithValue("@Title", addRequest.Title);
            paramCollection.AddWithValue("@Description", addRequest.Description);
            paramCollection.AddWithValue("@Summary", addRequest.Summary);
            paramCollection.AddWithValue("@Pay", addRequest.Pay);
            paramCollection.AddWithValue("@Slug", addRequest.Slug);
            paramCollection.AddWithValue("@StatusId", addRequest.StatusId);
            paramCollection.AddWithValue("@TechCompanyId", addRequest.TechCompanyId);
            paramCollection.AddWithValue("@UserId", userId);
            paramCollection.AddWithValue("@skillsBatch", skillsBatch);
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

        private static void AddCommonSearchParams(int pageIndex, int pageSize, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@PageIndex", pageIndex);
            paramCollection.AddWithValue("@PageSize", pageSize);
        }
    }
}
