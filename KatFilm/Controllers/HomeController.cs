using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Http;    // для обработки изображений
using Microsoft.AspNetCore.Hosting; // для обработки изображений
using System.IO;
using KatFilm.Models;
using System.Security.Claims;

namespace KatFilm.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly UserManager<User> _userManager; //сервис по управлению пользователями 
        private readonly SignInManager<User> _signInManager; //сервис позволяет аутентифицировать пользователя и устанавливать или удалять его куки.

        EFDbContext db;
        IWebHostEnvironment _appEnvironment;
        public HomeController(EFDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment)
        {
            this.db = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index(MainmessModel mess)
        {
            ViewBag.zag = "I";

            string nameuser = User.Identity.Name;

            if (nameuser != null)
            {
                int ind = nameuser.IndexOf('@');
                ViewBag.user = nameuser.Substring(0, ind);  // имя текущего пользователя
            }
            else ViewBag.user = "";
            return View(mess);
        }

        public IActionResult LoginOk()
        {
            return View();
        }
        // ---------- получение списка Фильмов
        public async Task<IActionResult> List(int page = 1)
        {
            if (_signInManager.IsSignedIn(User))  //  вход осуществлен
            {
              
                int pageSize = 8;   // количество элементов на странице

                IQueryable<Film> source = db.Films.OrderBy(p =>p.Name);
                var count = await source.CountAsync(); //общее количество элементов
                var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Films = items
                };
                ViewBag.zag = "L";
                string nameuser = User.Identity.Name;
                int ind = nameuser.IndexOf('@');
                ViewBag.user = nameuser.Substring(0, ind);

                return View(viewModel);
            }
            else
            {
                var mess = new MainmessModel { txt = "Вы не осуществили вход в систему" };
                return RedirectToAction("Index", "Home", mess);
            }
        }
        public async Task<IActionResult> Listd(int id) // id Films
        {
          
            string nameuser = User.Identity.Name;   // имя пользователя вошедшего в систему

            var film = await db.Films.FirstOrDefaultAsync(p => p.Id == id); // данные о фильме

            var user = await _userManager.FindByIdAsync(film.AspNetUserId);  // данные о пользователе загрузившем инфо о фильме
            if (user.UserName != nameuser) // инфо о фильме загрузил другой пользователь
            {
                return RedirectToAction("Detail", new { id = id });
            }

            return RedirectToAction("Edit", new { id = id });
        }
        public async Task<IActionResult> Edit(int? id)  // id Films
        {
            var film = await db.Films.FirstOrDefaultAsync(p => p.Id == id); // данные о фильме
            var user = await _userManager.FindByIdAsync(film.AspNetUserId); // данные о фильме загрузившем инфо о фильме
            int ind = user.UserName.IndexOf('@');
            ViewBag.avtor = user.UserName.Substring(0, ind);

            string nameuser = User.Identity.Name;
            ind = nameuser.IndexOf('@');
            ViewBag.user = nameuser.Substring(0, ind);
            ViewBag.zag = "I";
            return View(film);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, Film film)
        {
            if (uploadedFile != null)  // загружено изображение о Фильме
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName; //название файла
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копирует файл в поток
                }
               
                film.Path = path;
            }
            if (ModelState.IsValid)
            {
               
                db.Films.Update(film);
                await db.SaveChangesAsync();
          
                return RedirectToAction("Edit", new { id = film.Id });
            }
            return RedirectToAction("Edit"); //, new { id = film.Id });
        }
        // просмотр без редактирования
        public async Task<IActionResult> Detail(int? id)  // id Films
        {
            var film = await db.Films.FirstOrDefaultAsync(p => p.Id == id); // данные о фильме
            var user = await _userManager.FindByIdAsync(film.AspNetUserId); // данные о пользователе загрузившем данные о фильме
            int ind = user.UserName.IndexOf('@');
            ViewBag.avtor = user.UserName.Substring(0, ind);  // пользователь, загрузивший инфо о фильме

            string nameuser = User.Identity.Name;
            ind = nameuser.IndexOf('@');
            ViewBag.user = nameuser.Substring(0, ind);
            ViewBag.zag = "I";
            return View(film);
        }
        // добавление 
        public IActionResult Create()
                    {
            ViewBag.zag = "I";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile uploadedFile, Film film)
        {
            if (uploadedFile != null)  // загружено изображение о Фильме
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName; //название файла
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копирует файл в поток
                }
               
                film.Path = path;
            }
            if (ModelState.IsValid)   // ошибок валидации нет
            {
                

                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);  // Id текущего пользователя
               
                film.AspNetUserId = userId;
                db.Films.Add(film);
                await db.SaveChangesAsync();
         
                return RedirectToAction("Edit", new { id = film.Id });
            }
            return View(film);
        }
    
        [HttpPost]
        public IActionResult CheckName(int? id, string name, int year, string regisser)
        {
            if (id == null)
            {
                id = 0;
            }
            Film filmold = new Film();
            if (id != 0 )
            {      // корректировка
                filmold = db.Films.FirstOrDefault(p => p.Id == id);  // значения записи до корректировки   
            }
            if (((filmold.Name != name || filmold.Regisser != regisser || filmold.Year != year) && id != 0) || id == 0)
            {
                Film film = db.Films.FirstOrDefault(p => p.Name == name && p.Year == year && p.Regisser == regisser);
                if (film != null)
                    return Json(false);
            }
            return Json(true);
        }
        
    }
}
