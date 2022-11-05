using Core.Dtos;
using Core.Models;

namespace Core.Interfaces.IClients
{
    public interface IUserClient
    {
        Task<List<UserModel>>GetAll();
        Task<int> CreateUserAsync(UserModel userModel);   
        Task<UserModel?> FindOneAsync(int id);
        Task<string?> DeleteAsync(int id);
        Task<UserModel?> UpdateAsync(UserModel userModel);
        Task<SearchUserResponseDto?> Search(string searchField);
    }
}
