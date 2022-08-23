using Sabio.Models;
using Sabio.Models.Domain.Companies;
using Sabio.Models.Requests.Companies;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ICompanyService
    {
        int Add(CompanyAddRequest addRequest, int userId);
        Company Get(int id);
        List<Company> GetAll();
        Company GetBySlug(string slug);
        Paged<Company> Pagination(int pageIndex, int pageSize);
        Paged<Company> Search(int pageIndex, int pageSize, string query);
        void Update(CompanyUpdateRequest updateRequest, int userId);
        void UpdateStatusId(int id, string statusId, int userId);
    }
}