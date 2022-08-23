using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;

        public AddressApiController(IAddressService service
            , ILogger<AddressApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("")]
        public ActionResult<ItemsResponse<Address>> GetRandomAddresses()
        {
            ObjectResult result = null;
            try
            {
                List<Address> addresses = _service.GetRandomAddresses();
                ItemsResponse<Address> response = new ItemsResponse<Address>();
                response.Items = addresses;

                if (addresses == null)
                {
                    result = NotFound404(response);
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
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            ObjectResult result = null;
            try
            {
                Address address = _service.Get(id);

                ItemResponse<Address> response = new()
                {
                    Item = address
                };

                if (address == null)
                {
                    result = NotFound404(response);
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
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>();
                response.Item = id;

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


        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AddressUpdateRequest model)
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

    }
}
