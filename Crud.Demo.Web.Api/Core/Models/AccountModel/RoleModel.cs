using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AccountModel
{
    public class RoleModel : IdentityRole
    {
        public string RoleName { get; set; } = string.Empty;
    }
}
