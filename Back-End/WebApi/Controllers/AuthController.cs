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
    [Route("api/v1/Auth/[action]")]
    public class AuthController : BaseApiController
    {
        private readonly IAuth _process;
        public AuthController(IAuth process)
        {
            _process = process;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public IActionResult Login(User req)
        {
            return Response(new ApiResponse<Token>(_process.Login(req)));
        }
    }
}
