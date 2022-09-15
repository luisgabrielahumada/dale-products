using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Auth;
using Services.Models;
using WebApi.Code.Base;
using WebApi.Code.Response;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/sale")]
    public class SaleController : BaseApiController
    {
        private readonly ICustomer _process;
        public SaleController(ICustomer process)
        {
            _process = process;
        }

        // POST api/v1/sale/list
        [HttpPost("sale-list")]
        [ProducesResponseType(typeof(CustomersModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult List(Pagination pag)
        {
            return Response(new ApiResponse<CustomersModel>(_process.List(pag)));
        }

        // POST api/v1/sale/sale-process
        [HttpPost("sale-process")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Save(CustomerModel request)
        {
            return Response(new ApiResponse<int>(_process.Save(request)));
        }

        // GET api/v1/sale/sale-detail
        [HttpGet("sale-detail/{id}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetEvent([FromRoute] int id)
        {
            return Response(new ApiResponse<CustomerModel>(_process.Get(id)));
        }

        // POST api/v1/sale/sale-delete/{id}
        [HttpGet("sale-delete/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Delete([FromRoute]int id)
        {
            return Response(new ApiResponse<int>(_process.Delete(id)));
        }
    }
}
