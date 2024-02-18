using Api.Model;
using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;

namespace Api.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors("*")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("open-ai")]
        public async Task<ActionResult<string>>GetAnswer([FromBody]string question)
        {
            var apikey = _configuration.GetValue<string>("OpenAPIKey");
            string answer = string.Empty;
            var openApi = new OpenAIAPI(apikey);
            try
            {
                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = question;
                completion.Model = OpenAI_API.Models.Model.DavinciText;
                completion.MaxTokens = 8000;
                var json = JsonConvert.SerializeObject(question);
                var result = await openApi.Completions.CreateCompletionAsync(json);
                foreach(var item in result.Completions)
                {
                    answer = item.Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return Ok(answer);
            
        }

        
        [HttpGet("get-all")]
        public async Task<IActionResult> FindAll()
        {
            var result = await _userService.FindAllAsync();
            if (!result.Any())
            {
                return Ok(new List<UserModel>());
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

        [EnableCors("AllowMyOrigin")]
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

        //[Authorize(Policy =IdentityModel.AdminPolicyName)]
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

        [HttpGet("oneof")]
        public async Task<ActionResult>GetOneOf(string searchField, int id)
        {
            var result = await _userService.GetDataOneOf(searchField, id);
            if(result.IsT0)
            {
                return Ok(result.AsT0);
            }
            if(result.IsT1)
            {
                return Ok(result.AsT1);
            }
            return NotFound();
        }
    }
}
