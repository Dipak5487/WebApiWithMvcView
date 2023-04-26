using Core.Models.AccountModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        RoleManager<IdentityRole> _roleManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(RoleModel roleModel)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole() { Name = roleModel.RoleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(roleModel);
                }
                else
                {
                    return Ok(result.Errors.Select(x=>x.Description));
                }
                
            }
            return Ok(roleModel);
        }
        [HttpGet("get-all-roles")]
        public IActionResult ListRole()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }
    }
}
