using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<UserModel> Users
        {
            get;
            set;
        }
    }
}
