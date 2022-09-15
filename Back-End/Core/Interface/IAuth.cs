using Services.Models;
using Shared.Response;

namespace Services.Interface.Auth
{
    public interface IAuth
    {
        ServiceResponse<Token> Login(User req);
    }
}
