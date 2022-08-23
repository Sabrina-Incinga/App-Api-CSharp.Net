using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Events;
using Sabio.Models.Requests.Events;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventApiController : BaseApiController
    {
        private IEventService _service = null;
        private IAuthenticationService<int> _authService = null;

        public EventApiController(IEventService service
            , ILogger<EventApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(EventAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);

            }
            catch (System.Exception e)
            {
                base.Logger.LogError(e.ToString());
                ErrorResponse response = new ErrorResponse(e.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(EventUpdateRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
                SuccessResponse response = new SuccessResponse();

                result = Ok(response);

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
        public ActionResult<ItemResponse<Event>> GetById(int id)
        {
            ObjectResult result = null;
            try
            {
                Event anEvent = _service.Get(id);

                if (anEvent == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemResponse<Event> response = new ItemResponse<Event>() { Item = anEvent };
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

        [HttpGet("{slug}")]
        public ActionResult<ItemResponse<Event>> GetBySlug(string slug)
        {
            ObjectResult result = null;
            try
            {
                Event anEvent = _service.GetBySlug(slug);

                if (anEvent == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemResponse<Event> response = new ItemResponse<Event>() { Item = anEvent };
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

        [HttpGet("feeds")]
        public ActionResult<ItemsResponse<Event>> GetAll()
        {
            ObjectResult result = null;
            try
            {
                List<Event> events = _service.GetAllUpcoming();

                if (events == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemsResponse<Event> response = new ItemsResponse<Event>() { Items = events };
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

        [HttpGet("search")]
        public ActionResult<ItemResponse<Event>> Search(int pageIndex, int pageSize, string dateStart, string dateEnd)
        {
            ObjectResult result = null;
            try
            {
                Paged<Event> events = _service.SearchByDate(pageIndex, pageSize, dateStart, dateEnd);
                if (events == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemResponse<Paged<Event>> response = new ItemResponse<Paged<Event>>() { Item = events };
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
        
        [HttpGet("search/geo")]
        public ActionResult<ItemResponse<Event>> SearchGeo(int pageIndex, int pageSize, int radius, decimal startingLatitude, decimal startingLongitude)
        {
            ObjectResult result = null;
            try
            {
                Paged<Event> events = _service.SearchByGeo(pageIndex, pageSize, radius, startingLatitude, startingLongitude);
                if (events == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemResponse<Paged<Event>> response = new ItemResponse<Paged<Event>>() { Item = events };
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
        public ActionResult<ItemResponse<Event>> Pagination(int pageIndex, int pageSize)
        {
            ObjectResult result = null;
            try
            {
                Paged<Event> events = _service.Pagination(pageIndex, pageSize);
                if (events == null)
                {
                    result = NotFound404(new ErrorResponse("Record not found"));
                }
                else
                {
                    ItemResponse<Paged<Event>> response = new ItemResponse<Paged<Event>>() { Item = events };
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
    }
}
