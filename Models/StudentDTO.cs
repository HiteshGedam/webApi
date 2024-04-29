using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebApiApp.CustomeValidaor;

namespace WebApiApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is rquired")]
        [StringLength(20)]
        public string Name { get; set; }

        //[Range(10, 20)]
        //public int Age { get; set; }

        [EmailAddress(ErrorMessage = "please enter valid emial address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Student name is rquired")]
        public string Address { get; set; }

        //public int Password { get; set; }

        //[Compare(nameof(Password))]
        //public int ConfirmPassword { get; set; }

        //[DateCheck]
        public DateTime DOB { get; set; }
    }
}
