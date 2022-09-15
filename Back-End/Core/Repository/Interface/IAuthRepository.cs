using Services.Models;

namespace Services.Interface.Auth
{
    public interface IAuthRepository
    {
        Token LoginValidate(User req);
    }
}
 