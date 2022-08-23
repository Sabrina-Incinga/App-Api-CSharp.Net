using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Companies;
using Sabio.Models.Requests.Companies;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyApiController : BaseApiController
    {
        private ICompanyService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CompanyApiController(ICompanyService service
            , ILogger<CompanyApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CompanyAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(CompanyUpdateRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);

                SuccessResponse response = new();

                result = Ok(response);
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("")]
        public ActionResult<ItemsResponse<Company>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<Company> companies = _service.GetAll();
                ItemsResponse<Company> response = new ItemsResponse<Company>() { Items = companies };

                if (companies == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    result = Ok(response);
                }
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Company>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                Company companies = _service.Get(id);
                ItemResponse<Company> response = new ItemResponse<Company>() { Item = companies };

                if (companies == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    result = Ok(response);
                }

            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Company>>> GetPaginate(int pageIndex, int pageSize)
        {
            ObjectResult result = null;
            try
            {
                Paged<Company> pagedCompanies = _service.Pagination(pageIndex, pageSize);
                if (pagedCompanies == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<Company>> itemResponse = new ItemResponse<Paged<Company>>() { Item = pagedCompanies };
                    result = Ok(itemResponse);
                }
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Company>>> GetSearchPaginate(int pageIndex, int pageSize, string query)
        {
            ObjectResult result = null;
            try
            {
                Paged<Company> pagedSearch = _service.Search(pageIndex, pageSize, query);
                if (pagedSearch == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<Company>> itemResponse = new ItemResponse<Paged<Company>>() { Item = pagedSearch };
                    result = Ok(itemResponse);
                }
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPatch("{id:int}")]
        public ActionResult<SuccessResponse> UpdateStatusId(int id, string statusId)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.UpdateStatusId(id, statusId, userId);

                SuccessResponse response = new SuccessResponse();

                result = Ok(response);
            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
    }
}
