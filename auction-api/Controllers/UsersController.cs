using auction_api.IServices;
using auction_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace auction_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<LoginUser> GetItemsById([FromBody] LoginRequest content)
        {
            var loginUser =  _userService.GetLoginUser(content.Username, content.Password);
            if (loginUser != null) {
                return Ok(loginUser);
            }
            return BadRequest("Invalid Username or Password");
        }
    }
}
