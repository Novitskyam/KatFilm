using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace KatFilm.Models
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        //  public int RequiredLength { get; set; } // минимальная длина

        //public CustomPasswordValidator(int length)
        //   {
        //     RequiredLength = length;
        //  }
     //   EFDbContext db;
      //  public CustomPasswordValidator(EFDbContext context)
     //   {
       //     this.db = context;
      //  }
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            { // сравнение значений userName и пароля
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "Вы не можете использовать свой Email в качестве пароля"
                }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
