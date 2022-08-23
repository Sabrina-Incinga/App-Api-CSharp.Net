using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Jobs;
using Sabio.Models.Requests.Jobs;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobApiController : BaseApiController
    {
        private IJobService _service = null;
        private IAuthenticationService<int> _authService = null;

        public JobApiController(IJobService service
            , ILogger<JobApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(JobAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response= new ItemResponse<int>() { Item = id };

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
        public ActionResult<SuccessResponse> Update(JobUpdateRequest model)
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
        public ActionResult<ItemsResponse<Job>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<Job> jobs = _service.GetAll();
                ItemsResponse<Job> response = new ItemsResponse<Job>() { Items = jobs };

                if (jobs == null)
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
        public ActionResult<ItemResponse<Job>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                Job job = _service.Get(id);
                ItemResponse<Job> response = new ItemResponse<Job>() { Item = job };

                if (job == null)
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
        public ActionResult<ItemResponse<Paged<Job>>> GetPaginate(int pageIndex, int pageSize)
        {
            ObjectResult result = null;
            try
            {
                Paged<Job> pagedJobs = _service.Pagination(pageIndex, pageSize);
                if (pagedJobs == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<Job>> itemResponse = new ItemResponse<Paged<Job>>() { Item = pagedJobs };
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
        public ActionResult<ItemResponse<Paged<Job>>> GetSearchPaginate(int pageIndex, int pageSize, string query)
        {
            ObjectResult result = null;
            try
            {
                Paged<Job> pagedSearch = _service.Search(pageIndex, pageSize, query);
                if (pagedSearch == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<Job>> itemResponse = new ItemResponse<Paged<Job>>() { Item = pagedSearch };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            ObjectResult result = null;
            try
            {
                _service.Delete(id);

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
