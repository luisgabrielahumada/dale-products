using Common.Infrastructure;
using Services.Interface.Auth;
using Services.Models;
using Shared;
using Shared.Response;
using Shared.Security;
using System;

namespace Services.Auth
{
    public class AuthService : IAuth
    {
        public IAuthRepository _db;
        public readonly Settings _settings;
        public AuthService(IAuthRepository db)
        {
            _db = db;
        }

        public ServiceResponse<Token> Login(User req)
        {
            var sr = new ServiceResponse<Token>();
            try
            {
                sr.Data = _db.LoginValidate(req);
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
    }
}
