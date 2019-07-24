using System.ComponentModel.DataAnnotations;

namespace Documents.GitHub.DatingAPP.DatingApp.API.DTO
{
    public class UserForRegistrationDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Senha deve ter de 4 a 8 caracteres")]
        public string Password { get; set; }
    }
}