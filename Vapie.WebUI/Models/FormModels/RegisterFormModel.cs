using System.ComponentModel.DataAnnotations;

namespace Vapie.WebUI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifrələr Uyğun Deyil!")]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
