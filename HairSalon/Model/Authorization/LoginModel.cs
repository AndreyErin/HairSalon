using System.ComponentModel.DataAnnotations;

namespace HairSalon.Model.Authorization
{
    public class LoginModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Введите логин")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = string.Empty;
    } 
}
