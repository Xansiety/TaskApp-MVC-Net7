using System.ComponentModel.DataAnnotations;

namespace TaskApp.Models.Auth
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Error.Requerido")]
        [EmailAddress(ErrorMessage = "Error.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Error.Requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
