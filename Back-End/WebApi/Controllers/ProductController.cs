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
    [Route("api/v1/product")]
    public class ProductController : BaseApiController
    {
        private readonly IProduct _process;
        public ProductController(IProduct process)
        {
            _process = process;
        }

        // POST api/v1/product/product-list
        [HttpPost("product-list")]
        [ProducesResponseType(typeof(ProductsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult List(Pagination pag)
        {
            return Response(new ApiResponse<ProductsModel>(_process.List(pag)));
        }

        // POST api/v1/product/product-process
        [HttpPost("product-process")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Save(ProductModel request)
        {
            return Response(new ApiResponse<int>(_process.Save(request)));
        }

        // GET api/v1/product/product-detail
        [HttpGet("product-detail/{id}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetEvent([FromRoute] int id)
        {
            return Response(new ApiResponse<ProductModel>(_process.Get(id)));
        }

        // GET api/v1/product/product-delete/{id}
        [HttpGet("product-delete/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Delete([FromRoute]int id)
        {
            return Response(new ApiResponse<int>(_process.Delete(id)));
        }
    }
}
