using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using WebApi.Code.Response;

namespace WebApi.Code.Base
{
    public class BaseApiController : Controller
    {
        protected int GetAuthenticatedSessionId()
        {
            return HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.SerialNumber).Value.ToInt();
        }

        protected int GetAuthenticatedShopId()
        {
            return HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.SerialNumber).Value.ToInt();
        }

        protected new IActionResult Response(ApiResponse ar, bool notFoundOnError = false, bool returnAlways200 = false)
        {
            if (!ar.Status)
            {
                if (notFoundOnError)
                    return NotFound();

                return BadRequest(ar);
            }

            var content = ar.ReturnValue != 0 && ar.Data == null ? ar.ReturnValue : ar.Data;
            if (content == null)
                return returnAlways200 ? Ok() : ar.AsyncOperation ? (IActionResult) Accepted() : NoContent();

            return Ok(content);
        }

        protected new IActionResult Response<T>(ApiResponse<T> ar, bool notFoundOnError = false)
        {
            return Response(ar.ToGeneric(), notFoundOnError);
        }

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        protected async Task<string> GetRawBodyStringAsync(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(Request.Body, encoding))
                return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<byte[]> GetRawBodyBytesAsync()
        {
            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}