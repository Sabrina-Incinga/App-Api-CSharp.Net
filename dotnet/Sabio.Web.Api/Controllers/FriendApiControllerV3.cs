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
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV3(IFriendService service
            , ILogger<FriendApiControllerV3> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendAddRequestV3 model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.AddV3(model, userId);
                ItemResponse<int> response = new() { Item = id };

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
        public ActionResult<SuccessResponse> Update(FriendUpdateRequestV3 model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.UpdateV3(model, userId);

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
        public ActionResult<ItemsResponse<FriendV3>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<FriendV3> friends = _service.GetAllV3();
                ItemsResponse<FriendV3> response = new ItemsResponse<FriendV3>() { Items = friends };

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
        public ActionResult<ItemResponse<FriendV3>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                FriendV3 friend = _service.GetV3(id);
                ItemResponse<FriendV3> response = new ItemResponse<FriendV3>() { Item = friend };

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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> GetPaginate(int pageIndex, int pageSize)
        {
            ObjectResult result = null;
            try
            {
                Paged<FriendV3> pagedFriends = _service.PaginationV3(pageIndex, pageSize);
                if (pagedFriends == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV3>> itemResponse = new ItemResponse<Paged<FriendV3>>() { Item = pagedFriends };
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
        public ActionResult<ItemResponse<Paged<FriendV3>>> GetSearchPaginate(int pageIndex, int pageSize, string query)
        {
            ObjectResult result = null;
            try
            {
                Paged<FriendV3> pagedSearch = _service.SearchV3(pageIndex, pageSize, query);
                if (pagedSearch == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV3>> itemResponse = new ItemResponse<Paged<FriendV3>>() { Item = pagedSearch };
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
                _service.DeleteV3(id);

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
