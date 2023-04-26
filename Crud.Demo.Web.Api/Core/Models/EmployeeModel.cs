using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public Department? Department { get; set; }

        [Display(Name = "Official Email")]
        [Required]
        // [RegularExpression("@`^[await-zA-Z0-9_.+-]@[a-zA-Z0-9+-]+\\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid Email Format")]
        //[Remote(action: "IsExistEmail", controller: "Account")]
        public string Email { get; set; }

        public string PhotoPath { get; set; }


    }

    public enum Department
    {
        NON, IT, HR
    }
}
