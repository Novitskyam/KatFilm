using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KatFilm.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} должен иметь минимум {2} и максимум {1} символов.", MinimumLength = 4)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
