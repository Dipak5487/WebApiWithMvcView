using Core.Dtos;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<int>CreateUserAsync(UserModel userModel);
        Task<List<UserModel>> FindAllAsync();
        Task<UserModel?>FindOneAsync(int id);
        Task<string> DeleteAsync(int id);
        Task<UserModel> UpdateAsync(int id, UserModel userModel);
        Task<SearchUserResponseDto> Search(string searchField);
    }
}
