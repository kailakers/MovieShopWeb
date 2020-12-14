using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieShop.Core.Models;
using MovieShop.Core.ServiceInterfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MovieShop.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterRequestModel userRegisterRequestModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUser(userRegisterRequestModel);
                return Ok();
            }
            else
                return BadRequest(new {message="Please Correct the Information!" });
        }

        // [HttpGet]
        // [Route("{id:int}"), Name = "GetUser"]
        // public async Task<IActionResult> GetById(int id)
        // {
        //     
        // }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest, string returnUrl = null)
        {
            //returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid)
                return BadRequest(new {message = "Username/Password is incorrect!"});
            var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return Unauthorized();

            var token = GenerateJWT(user);

            return Ok(new {token});
    }

        private string GenerateJWT(UserLoginResponseModel userLoginResponseModel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userLoginResponseModel.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, userLoginResponseModel.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, userLoginResponseModel.LastName),
                new Claim(JwtRegisteredClaimNames.Email, userLoginResponseModel.Email),
                new Claim(ClaimTypes.Role, userLoginResponseModel.Roles.Count>0?userLoginResponseModel.Roles[0]:"User")
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSetting:PrimaryKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddHours(_configuration.GetValue<double>("TokenSetting:ExpirationHours"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = _configuration["TokenSetting: Audience"],
                Issuer = _configuration["TokenSetting: Issuer"],
                SigningCredentials = credentials,
                Expires = expires
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(encodedToken);
        }
    }
}