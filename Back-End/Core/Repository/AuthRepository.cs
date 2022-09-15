using Common.Infrastructure;
using Common.Infrastructure.Helper;
using Core.Component.Library.SQL;
using Services.Database;
using Services.Interface.Auth;
using Services.Models;
using System;
using System.Linq;

namespace Services.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        readonly AppDatabase db;
        Settings _settings;
        public AuthRepository(Settings settings)
        {
            _settings = settings;
            db = new AppDatabase(settings.ConnectionStrings.ConnectionString);
        }
        public Token LoginValidate(User req)
        {
            Token resp = new Token();
            var security = Encription.GetInstance(_settings.Configuration.Token);
            var _password = security.Encrypt(req.Password);
            resp = new Execute(db, "DB_Login_Validate",
                              new
                              {
                                  login = req.Login,
                                  password = _password
                              }).Procedure<Token>()
                                .FirstOrDefault();
            resp.SessionId = Guid.NewGuid().ToString();
            resp._Token = new JwtSecurity.JwtSecurityWeb().GenerateJSONWebToken(resp, _settings);
            return resp;
        }
    }
}