using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.DbContexts
{
    public class UserDbContext : IdentityDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<EmployeeModel> employees { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
