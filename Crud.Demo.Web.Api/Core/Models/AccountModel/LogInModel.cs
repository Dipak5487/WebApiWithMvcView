using System.ComponentModel.DataAnnotations;

namespace Core.Models.AccountModel
{
    public class LogInModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember Me")]
        public bool RememverMe { get; set; }
    }
}
