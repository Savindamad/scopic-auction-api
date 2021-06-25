using auction_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auction_api.IServices
{
    public interface IUserService
    {
        LoginUser GetLoginUser(string username, string password);
        string generateJwtToken(UserInfo user);
    }
}
