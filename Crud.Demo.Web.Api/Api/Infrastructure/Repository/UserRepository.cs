using Api.DbContexts;
using Core.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;
        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<int> CreateUserAsync(UserModel userModel)
        {
            try
            {
                await _userDbContext.Users.AddAsync(userModel);
                var result = await _userDbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {ex}", ex.Message);
            }

            return 0;
        }

        public async Task<string> DeleteAsync(int id)
        {
            var userModel = await _userDbContext.Users.FindAsync(id);
            if (userModel != null)
            {
                _userDbContext.Users.Remove(userModel);
                await _userDbContext.SaveChangesAsync();
                return "Remvoe Success";
            }

            return "Recod not found in database";
        }

        public async Task<List<UserModel>> FindAllAsync()
        {
            var result = await _userDbContext.Users.ToListAsync();
            if (result.Any())
            {
                return result;
            }

            return new List<UserModel>();
        }

        public async Task<UserModel?> FindOneAsync(int id)
        {
            var result = await _userDbContext.Users.FindAsync(id);
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<UserModel> UpdateAsync(int id, UserModel userModel)
        {
            var result = await _userDbContext.Users.FindAsync(id);
            if (result != null && userModel != null)
            {
                result.Name = userModel.Name;
                result.EmailId = userModel.EmailId;
                result.MobileNumber = userModel.MobileNumber;
                result.Country = userModel.Country;
                result.DOB = userModel.DOB;
                _userDbContext.Users.Update(result);
                await _userDbContext.SaveChangesAsync();
                return result;
            }

            return new UserModel();
        }

        public async Task<SearchUserResponseDto> Search(string searchFeild)
        {
            var userModels = new SearchUserResponseDto();
            if (int.TryParse(searchFeild, out _))
            {
                var result = await _userDbContext.Users.Where(x => x.Id == Convert.ToInt32(searchFeild)).ToListAsync();
                if (result.Any())
                {
                    userModels.UserModels = result;
                    userModels.TotalCount = result.Count;
                    return userModels;
                }
            }
            if (!string.IsNullOrEmpty(searchFeild))
            {
                var result = await _userDbContext.Users.Where(x => x.Name.Contains(searchFeild)
                || x.Country.Contains(searchFeild)
                || x.EmailId.Contains(searchFeild)
                || x.MobileNumber.Contains(searchFeild)).ToListAsync();

                if (result.Any())
                {
                    userModels.UserModels = result;
                    userModels.TotalCount = result.Count;
                    return userModels;
                }
            }
            return new SearchUserResponseDto();
        }
    }
}
