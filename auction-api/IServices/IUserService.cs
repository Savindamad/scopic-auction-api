using auction_api.Models;

namespace auction_api.IServices
{
    public interface IUserService
    {
        LoginUser GetLoginUser(string username, string password);
        string GenerateJwtToken(UserInfo user);
        UserConfig GetUserConfig(int userId);
        UserConfig UpdateUserConfig(UserConfig config);

    }
}
