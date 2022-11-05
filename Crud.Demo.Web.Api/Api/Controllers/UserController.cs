using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<UserModel>>> FindAll()
        {
            var result = await _userService.FindAllAsync();
            if (!result.Any())
            {
                return NotFound("Records Not found in database");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> FindOneAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than zero");
            }
            var result = await _userService.FindOneAsync(id);
            if (result == null)
            {
                return NotFound("Record Not found in database");
            }

            return Ok(result);
        }

        [HttpPost("user-create")]
        public async Task<ActionResult<int>> CreateUser([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("User Model Can't be null");
            }
            var result = await _userService.CreateUserAsync(userModel);
            if (result > 0)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPut("update")]
        public async Task<ActionResult<UserModel>> UpdateAsync(UserModel userModel)
        {
            if (userModel == null || userModel.Id <= 0)
            {
                return BadRequest("Invalid id and Data");
            }
            var result = await _userService.UpdateAsync(userModel.Id, userModel);
            if (result == null)
            {
                return NotFound("Record Not found in database");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string?>> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid id");
            }
            var result = await _userService.DeleteAsync(id);

            return Ok(result);
        }

        [HttpGet("search/{seaarchField}")]
        public async Task<ActionResult<SearchUserResponseDto>> Search(string seaarchField)
        {
            if (string.IsNullOrEmpty(seaarchField))
            {
                return BadRequest("Invalid operation");
            }
            var result = await _userService.Search(seaarchField);
            if (!result.UserModels.Any())
            {
                return NotFound("Not Found");
            }

            return Ok(result);
        }
    }
}
