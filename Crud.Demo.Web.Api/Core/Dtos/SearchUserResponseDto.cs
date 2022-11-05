using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class SearchUserResponseDto
    {
        public SearchUserResponseDto()
        {
            UserModels = new List<UserModel>();
        }
        public List<UserModel> UserModels { get; set; }
        public int TotalCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchField { get; set; }
    }
}
