using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KatFilm.Models
{
    public class Film
    {
       [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
     
        public string Regisser { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        // [RegularExpression("[0-9]", ErrorMessage = "Некорректный год")]
        [Range(1896, 3000, ErrorMessage = "Недопустимый год")]
        [Remote("CheckName", "Home", HttpMethod = "Post", AdditionalFields = "Id,Name,Year,Regisser", ErrorMessage = "Уже есть такой фильм ")]
        public int Year { get; set; }

        //  [MaxLength(100)]
        public string Opis { get; set; }
        public string AspNetUserId { get; set; }
        public string Path { get; set; }
       
    }
}
