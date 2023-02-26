using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RegForm.Models
{
    public class RegFormModel
    {
        [Key]

        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "Enter the first name...")]
        public string firstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Enter the last name...")]
        public string lastName { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Enter the email...")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }

        [Display(Name = "Mobile:")]
        [Required(ErrorMessage = "Enter the mobile number...")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string mobile { get; set; }

        [Display(Name = "Address:")]
        [Required(ErrorMessage = "Enter the address...")]
        public string address { get; set; }

        [Display(Name = "State:")]
        [Required(ErrorMessage = "Select the state...")]
        public string state { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Enter the city...")]
        public string city { get; set; }

        [Display(Name = "ZipCode:")]
        [Required(ErrorMessage = "Enter the zipcode...")]
        public string zipCode { get; set; }
    }
}

