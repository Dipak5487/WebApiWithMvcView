using Core.Models.AccountModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace Api.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserAcoountModel userAcoountModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = userAcoountModel.Email, Email = userAcoountModel.Email };
                var result = await _userManager.CreateAsync(user, userAcoountModel.Password);
                if (result.Succeeded)
                {
                    return Ok("User Register");
                }                
            }
            return BadRequest("Email Id or Password Incorrect");
        }

        [HttpPost("log-out")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("is-exist-email")]
        [AllowAnonymous]
        public async Task<IActionResult> IsExistEmail(string email)
        {
            var userEmail = await _userManager.FindByEmailAsync(email);
            if (userEmail == null)
            {
                return Ok(true);

            }
            return Ok($"Emai {email} is already exist");

        }
        [HttpPost("log-in")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInModel logInModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = new Microsoft.AspNetCore.Identity.SignInResult();
            if (ModelState.IsValid)
            {
                signInResult = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, logInModel.RememverMe, false);
                if (signInResult.Succeeded)
                {
                    return Ok(signInResult);
                }
            }

            return Ok(signInResult);
        }
    }
}

