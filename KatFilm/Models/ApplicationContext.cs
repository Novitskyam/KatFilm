using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace KatFilm.Models
{

    public class ApplicationContext : IdentityDbContext<User>
    {
       // public DbSet<Film> Films { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           Database.EnsureCreated();
        }
    }
}
