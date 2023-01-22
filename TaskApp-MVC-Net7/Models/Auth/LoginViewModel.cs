using System.ComponentModel.DataAnnotations;

namespace TaskApp.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Error.Requerido")]
        [EmailAddress(ErrorMessage = "Error.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Error.Requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}
