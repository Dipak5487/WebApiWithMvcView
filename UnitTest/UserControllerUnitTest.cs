using Api.Controllers;
using Api.Infrastructure.Services;
using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTest
{
    public class UserControllerUnitTest
    {
        private readonly UserController _userController;
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IConfiguration> _config;
        public UserControllerUnitTest()
        {
            _userRepository = new Mock<IUserRepository>();

            _userRepository.Setup(x => x.CreateUserAsync(It.IsAny<UserModel>())).ReturnsAsync(1);
            _userRepository.Setup(x => x.FindAllAsync()).ReturnsAsync(GetModelList());
            _userRepository.Setup(x => x.FindOneAsync(It.IsAny<int>())).ReturnsAsync(GetModelList().First());
            _userRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync("Success");
            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(),It.IsAny<UserModel>())).ReturnsAsync(GetModelList().First());
            _userRepository.Setup(x => x.Search(It.IsAny<string>())).ReturnsAsync(new SearchUserResponseDto() { TotalCount=3,UserModels= GetModelList() });
            _config = new Mock<IConfiguration>();
             _userService = new UserService(_userRepository.Object);
            _userController = new UserController(_userService, _config.Object);
        }
        [Fact]
        public async Task CreateUser()
        {
            var result = await _userController.CreateUser(GetModelList().First());
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }

        [Fact]
        public async Task FindAllUser()
        {
            var result = await _userController.FindAll();
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }
        [Fact]
        public async Task FindOneUser()
        {
            var result = await _userController.FindOneAsync(1);
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }
        [Fact]
        public async Task BadRequestIfIdIsInvalidToFindById()
        {
            var result = await _userController.FindOneAsync(0);
            var badObject = result.Result as BadRequestObjectResult;
            Assert.True(badObject?.StatusCode == 400);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var result = await _userController.Delete(1);
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }

        [Fact]
        public async Task BadRequestIfIdIsInvalidDeleteById()
        {
            var result = await _userController.Delete(0);
            var badObject = result.Result as BadRequestObjectResult;
            Assert.NotNull(badObject);
            Assert.True(badObject?.StatusCode == 400);
        }

        [Fact]
        public async Task UpdateUser()
        {
            var result = await _userController.UpdateAsync(GetModelList().First());
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }

        [Fact]
        public async Task SearchUser()
        {
            var result = await _userController.Search("text");
            var okObject = result.Result as OkObjectResult;
            Assert.NotNull(okObject);
            Assert.True(okObject?.StatusCode == 200);
        }

        public List<Core.Models.UserModel> GetModelList()
        {
            var models = new List<Core.Models.UserModel>()
            {
                new Core.Models.UserModel()
                {
                    Country="India",
                    DOB= DateTime.Now,
                    EmailId="example@gmail.com",
                    Id=1,
                    MobileNumber="8858299210",
                    Name="dipak"
                }
            };
            return models;
        }
    }
}