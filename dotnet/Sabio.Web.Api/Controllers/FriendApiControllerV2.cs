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
    [Route("api/v2/friends")]
    [ApiController]
    public class FriendApiControllerV2 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV2(IFriendService service
            , ILogger<FriendApiControllerV2> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendAddRequestV2 model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.AddV2(model, userId);
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
        public ActionResult<SuccessResponse> Update(FriendUpdateRequestV2 model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.UpdateV2(model, userId);

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
        public ActionResult<ItemsResponse<FriendV2>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<FriendV2> friends = _service.GetAllV2();
                ItemsResponse<FriendV2> response = new ItemsResponse<FriendV2>() { Items = friends };

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
        public ActionResult<ItemResponse<FriendV2>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                FriendV2 friend = _service.GetV2(id);
                ItemResponse<FriendV2> response = new ItemResponse<FriendV2>() { Item = friend };

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
        public ActionResult<ItemResponse<Paged<FriendV2>>> GetPaginate(int pageIndex, int pageSize)
        {
            ObjectResult result = null;
            try
            {
                Paged<FriendV2> pagedFriends = _service.PaginationV2(pageIndex, pageSize);
                if (pagedFriends == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV2>> itemResponse = new ItemResponse<Paged<FriendV2>>() { Item = pagedFriends };
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
        public ActionResult<ItemResponse<Paged<FriendV2>>> GetSearchPaginate(int pageIndex, int pageSize, string query)
        {
            ObjectResult result = null;
            try
            {
                Paged<FriendV2> pagedSearch = _service.SearchV2(pageIndex, pageSize, query);
                if (pagedSearch == null)
                {
                    result = NotFound404(new ErrorResponse("Resource not found"));
                }
                else
                {
                    ItemResponse<Paged<FriendV2>> itemResponse = new ItemResponse<Paged<FriendV2>>() { Item = pagedSearch };
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
                _service.DeleteV2(id);

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
