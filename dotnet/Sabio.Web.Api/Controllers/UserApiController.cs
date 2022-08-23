using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUserServiceV1 _service = null;

        public UserApiController(IUserServiceV1 service
            , ILogger<UserApiController> logger) : base(logger)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            ObjectResult result = null;

            try
            {
                List<User> users = _service.GetAll();
                ItemsResponse<User> response = new ItemsResponse<User>() { Items = users };

                if (users == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    result = Ok(response);
                }


            }
            catch (System.Exception e)
            {
                base.Logger.LogError(e.ToString());
                ErrorResponse response = new ErrorResponse(e.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> GetById(int id)
        {
            ObjectResult result = null;

            try
            {
                User user = _service.Get(id);
                ItemResponse<User> response = new ItemResponse<User>() { Item = user };

                if (user == null)
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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = userId };

                result = Created201(response);

            }
            catch (System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());

                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                result = StatusCode(500, errorResponse);
            }

            return result;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<SuccessResponse> Update(UserUpdateRequest model)
        {
            ObjectResult result = null;

            try
            {
                _service.Update(model);

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
            catch(System.Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
    }
}
