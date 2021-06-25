using auction_api.IServices;
using auction_api.Models;
using System;
using System.Linq;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace auction_api.Services
{

    public class UserService : IUserService
    {
        AuctionDbContext _dbContext;
        public IConfiguration _configuration;
        public UserService(AuctionDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _configuration = config;
        }

        public LoginUser GetLoginUser(string username, string password)
        {
            var user = _dbContext.UserInfos.FirstOrDefault(x => x.Username == username);
            if (user.Password == password) {
                var loginUser = new LoginUser();
                loginUser.User = user;
                loginUser.Token = GenerateJwtToken(user);
                return loginUser;
            }
            return null;
        }

        public UserConfig GetUserConfig(int userId) {
            var userConfig = _dbContext.UserConfigs.FirstOrDefault(x => x.UserId == userId);
            return userConfig;
        }

        public UserConfig UpdateUserConfig(UserConfig config) {
            try
            {
                _dbContext.Update(config);
                _dbContext.SaveChanges();
                return config;
            }
            catch
            {
                throw;
            }
        }

        public string GenerateJwtToken(UserInfo user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
