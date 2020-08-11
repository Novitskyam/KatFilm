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
            // ���������� ApplicationDbContext ��� �������������� � ����� ������ ������� �������
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<EFDbContext>(options =>options.UseSqlServer(connection));
            // ���������� �������� Idenity

            //   services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            // services.AddTransient<IUserValidator<User>, CustomUserValidator>();
            //  services.AddTransient<CustomUserValidator>();
            //   services.AddTransient<IPasswordValidator<User>,
            //         CustomPasswordValidator>(serv => new CustomPasswordValidator(6));  // �������� ����� �����������


            //       opts.Password.RequiredLength = 4; // ����������� ����� 


            //      opts.Password.RequiredUniqueChars = 6;  // �� ����� 6 ���������� ��������
            //     //  opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz"; // ���������� �������
            //     })
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = false; // ��������� �� �����
                opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                opts.User.RequireUniqueEmail = false;    // ���������� email
                opts.SignIn.RequireConfirmedAccount = false;
                opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddPasswordValidator<CustomPasswordValidator>() // ��������� ������ ������  ���������� ������
                .AddDefaultTokenProviders(); // ����������� ���������������� ��������� �������, ������� ���������� � ������ ��� �������������

            services.AddControllersWithViews();  //  ���������� �������� MVC

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

            app.UseAuthentication();    // ����������� ��������������
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
