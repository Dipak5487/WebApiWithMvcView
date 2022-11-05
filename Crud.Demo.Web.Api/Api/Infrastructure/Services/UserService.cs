using Core.Dtos;
using Core.Interfaces;
using Core.Models;

namespace Api.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            var result = await _userRepository.FindAllAsync();

            return result;
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
    }
}
