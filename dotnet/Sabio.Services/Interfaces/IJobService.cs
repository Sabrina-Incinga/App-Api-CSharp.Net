using Sabio.Models;
using Sabio.Models.Domain.Jobs;
using Sabio.Models.Requests.Jobs;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IJobService
    {
        int Add(JobAddRequest addRequest, int userId);
        void Delete(int id);
        Job Get(int id);
        List<Job> GetAll();
        Paged<Job> Pagination(int pageIndex, int pageSize);
        Paged<Job> Search(int pageIndex, int pageSize, string query);
        void Update(JobUpdateRequest updateRequest, int userId);
    }
}