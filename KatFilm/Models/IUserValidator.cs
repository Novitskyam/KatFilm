using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


//namespace KatFilm.Models
//{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user);
    }
//}
