using Api.Model;
using Core.Dtos;
using Core.Models.AccountModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Api.Controllers.AccountControllers
{
    [EnableCors("*")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthrizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthrizationController(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _config = configuration;
            _signInManager = signInManager;
        }
        [HttpPost("GetToken")]
        public async Task<ActionResult<AuthenticateResponse>> GetToken([FromBody] LogInModel logInModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = new Microsoft.AspNetCore.Identity.SignInResult();
            if (ModelState.IsValid)
            {
                signInResult = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, logInModel.RememverMe, false);
                if (signInResult.Succeeded)
                {
                    var loginCread = new UserLogInResponse();
                    loginCread.Id = Guid.NewGuid().ToString();
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSetting:Key"]!));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                      new Claim(ClaimTypes.NameIdentifier,logInModel.Email),
                      new Claim(ClaimTypes.Role,""),
                      new Claim(JwtRegisteredClaimNames.Jti,loginCread.Id),
                      new Claim(JwtRegisteredClaimNames.Sub, logInModel.Email),
                      new Claim(JwtRegisteredClaimNames.Email,logInModel.Email),
                      new Claim("userid", logInModel.Email)
                     };
                    var token = new JwtSecurityToken(
                        _config["JwtSetting:Issuer"],
                        _config["JwtSetting:Audiance"],
                        claims,
                        expires: DateTime.Now.AddMinutes(15),
                        signingCredentials: credentials
                        );
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    loginCread.Email = logInModel.Email;
                    loginCread.Roll = "Admin";
                    loginCread.RememverMe = logInModel.RememverMe;
                    return Ok(new AuthenticateResponse(loginCread, jwt, signInResult));
                }
            }

            return new UnauthorizedResult();
        }
    }
}
