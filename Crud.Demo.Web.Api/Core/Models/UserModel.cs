using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Name = string.Empty;
            EmailId = string.Empty;
            Country = string.Empty;
            MobileNumber = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }        
        [Required, DataType(DataType.EmailAddress,ErrorMessage ="Provide valid Email id.")]
        public string EmailId { get; set; }
        public string Country { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Error : Provide valid Mob No.")]
        [RegularExpression("^([0]|\\+91)?[789]\\d{9}$", ErrorMessage="Invalid Mobile Number")]
        public string MobileNumber { get; set; }
    }
}
