using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KatFilm.Models;

using Microsoft.AspNetCore.Identity.UI;

namespace KatFilm
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       // public int length=1;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
          string connection = "Server=(localdb)\\mssqllocaldb;Database=KatFilm;Trusted_Connection=True";
          
       //  string connection= "Server = SHURA-PC\\SQLEXPRESS; Database = KatFilm; Trusted_Connection=true";
            // добавление ApplicationDbContext для взаимодействия с базой данных учетных записей
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<EFDbContext>(options =>options.UseSqlServer(connection));
            // добавление сервисов Idenity

            //   services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            // services.AddTransient<IUserValidator<User>, CustomUserValidator>();
            //  services.AddTransient<CustomUserValidator>();
            //   services.AddTransient<IPasswordValidator<User>,
            //         CustomPasswordValidator>(serv => new CustomPasswordValidator(6));  // передаем длину минимальную


            //       opts.Password.RequiredLength = 4; // минимальная длина 


            //      opts.Password.RequiredUniqueChars = 6;  // не менее 6 кникальных символов
            //     //  opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz"; // допустимые символы
            //     })
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = false; // требуются ли цифры
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.User.RequireUniqueEmail = false;    // уникальный email
                opts.SignIn.RequireConfirmedAccount = false;
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddPasswordValidator<CustomPasswordValidator>() // добавляем сервис своего  валидатора пароля
                .AddDefaultTokenProviders(); // добавляется функциональность генерации токенов, которые отсылаются в письме для подтверждения

            services.AddControllersWithViews();  //  добавление сервисов MVC

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
