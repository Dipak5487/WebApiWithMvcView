using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using OneOf;
using Polly;
using Polly.Retry;

namespace Api.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private const int MaxRetry = 3;
        private readonly IUserRepository _userRepository;
        private readonly AsyncRetryPolicy _asyncRetryPolicy;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _asyncRetryPolicy = Policy.Handle<Exception>(exception =>
            {
                return exception.Message != "Do Something common ex message";
            }).WaitAndRetryAsync(MaxRetry, time => TimeSpan.FromMilliseconds(time * 100));
        }

        public async Task<int> CreateUserAsync(UserModel userModel)
        {
            if (userModel != null)
            {
                return await _userRepository.CreateUserAsync(userModel);
            }

            return 0;
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0) return "Provide valid Id";
            var result = await _userRepository.DeleteAsync(id);

            return result;
        }

        public async Task<List<UserModel>> FindAllAsync()
        {
            return await _asyncRetryPolicy.ExecuteAsync(async () =>
            {
                return await _userRepository.FindAllAsync();
            });
        }

        public async Task<UserModel?> FindOneAsync(int id)
        {
            if (id <= 0) return null;
            var result = await _userRepository.FindOneAsync(id);

            return result;
        }

        public async Task<UserModel?> UpdateAsync(int id, UserModel userModel)
        {
            if (userModel == null || id <= 0) return null;
            var result = await _userRepository.UpdateAsync(id, userModel);

            return result;
        }

        public async Task<SearchUserResponseDto> Search(string searchField)
        {
            var result = await _userRepository.Search(searchField);
            return result;
        }

        public async Task<OneOf<SearchUserResponseDto, UserModel>> GetDataOneOf(string searchField,int id)
        {
            var result = await _userRepository.Search(searchField);
            if (result != null && result.UserModels.Any()) return result;
            var userModel = await _userRepository.FindOneAsync(id);
            return userModel == null ? new UserModel() : userModel;
        }
    }
}
