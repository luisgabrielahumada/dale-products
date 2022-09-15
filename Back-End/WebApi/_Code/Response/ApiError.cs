using Shared.Response;

namespace WebApi.Code.Response
{
    public class ApiError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ApiError(ServiceError x)
        {
            ErrorCode = x.ErrorCode;
            ErrorMessage = x.ErrorMessage;
        }
    }
}