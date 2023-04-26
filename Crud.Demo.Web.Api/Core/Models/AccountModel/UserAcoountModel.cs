using Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Models.AccountModel
{
    public class UserAcoountModel
    {
        [Required]
        [EmailAddress]
        //[Remote(action: "IsExistEmail", controller: "account")]
        [ValidEmailDomain(allowedDomain: "dipak.com",
        ErrorMessage = "Provide Valid Domain dipak.com")]
        public string Email { get; set; } = string.Empty;
        [Required][DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not matched")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
