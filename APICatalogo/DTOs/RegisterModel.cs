using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "userName is required!")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string? Password { get; set; }   

        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string? Email { get; set; }
    }
}
