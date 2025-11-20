using System.ComponentModel.DataAnnotations;

namespace JsonWebTokenwithIdentity.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }=string.Empty;  
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        //[DataType(DataType.Password)]
        //[StringLength(100,ErrorMessage ="the password will be at least 8 character to maximum 100 character",MinimumLength =8)]
        
        public string Password { get; set; } = string.Empty;

        //[DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="the Password and confirm password do to match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [Display(Name="Role Name")]
        public string Role { get; set; } = string.Empty;
    }
}
