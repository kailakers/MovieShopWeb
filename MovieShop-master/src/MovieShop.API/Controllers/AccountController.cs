using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AccountController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUser")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserDetails(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUserAsync([FromBody] UserRegisterRequestModel user)
        {
            var createdUser = await _userService.CreateUser(user);
            return CreatedAtRoute("GetUser", new {id = createdUser.Id}, createdUser);
        }

        [HttpGet]
        public async Task<ActionResult> EmailExists([FromQuery] string email)
        {
            var user = await _userService.GetUser(email);
            return Ok(user == null ? new {emailExists = false} : new {emailExists = true});
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequestModel loginRequest)
        {
            var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null) return Unauthorized();

            return Ok(new {token = _jwtService.GenerateToken(user)});
        }
    }
}