using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthrizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthrizationController(IConfiguration configuration)
        {
            _config = configuration;
        }
        [HttpPost("GetToken")]
        public IActionResult GetToken([FromBody] AuthModel authModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSetting:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,authModel.Username),
                new Claim(ClaimTypes.Role,authModel.Role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, authModel.Emailid),
                new Claim(JwtRegisteredClaimNames.Email,authModel.Emailid),
                new Claim("userid", authModel.Emailid)
            };
            var token = new JwtSecurityToken(
                _config["JwtSetting:Issuer"],
                _config["JwtSetting:Audiance"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }
    }
}
