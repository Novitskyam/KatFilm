using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace KatFilm.Models
{
    public class EFDbContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
       // public DbSet<FileModel> Files { get; set; }
        public EFDbContext(DbContextOptions<EFDbContext> options)
               : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
