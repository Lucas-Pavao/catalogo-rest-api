using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class LoginModel
    {
        [Required (ErrorMessage = "userName is required!")]
        public string? userName { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string? password { get; set; }
    }
}
