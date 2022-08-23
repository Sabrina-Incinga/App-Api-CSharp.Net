using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiController(IFriendService service
            , ILogger<FriendApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendAddRequest model)
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
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(FriendUpdateRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);

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

        [HttpGet("")]
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<Friend> friends = _service.GetAll();
                ItemsResponse<Friend> response = new ItemsResponse<Friend>() { Items = friends };

                if (friends == null)
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
        public ActionResult<ItemResponse<Friend>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                Friend friend = _service.Get(id);
                ItemResponse<Friend> response = new ItemResponse<Friend>() { Item = friend };

                if (friend == null)
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
