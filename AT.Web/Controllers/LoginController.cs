using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AT.Data.Models;
using AT.Repo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AT.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public LoginController(IConfiguration config, ILoginRepository<User> loginRepository)
        {
            _config = config;
            _loginRepository = loginRepository;
        }

        private IConfiguration _config;
        private readonly ILoginRepository<User> _loginRepository;

        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            User login = new User()
            {
                UserName = username,
                Password = password
            };

            IActionResult response = Unauthorized();

            var userinfo = AuthenticateUser(login);
            if (userinfo != null)
            {
                var tokenStr = GenerateJSONWebToken(userinfo);
                response = Ok(new { token = tokenStr });
            }
            return response;
        }

        private User AuthenticateUser(User login)
        {
            return _loginRepository.Get(login);
        }

        private string GenerateJSONWebToken(User userinfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, userinfo.Password),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome to " + userName;
        }

        [Authorize]
        [HttpGet("GetUser")]
        public ActionResult<User> Get([FromBody] User userinfo)
        {
            var user = new User();

            user = _loginRepository.Get(userinfo);

            if (user == null)
            {
                return NotFound("The User is not in the database");
            }

            return Ok(user);
        }
    }
}