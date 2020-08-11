using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatFilm.Models
{
    public class IndexViewModel
    { // инкапсулирование информацию о пагинации и полученных фильмах:
        public IEnumerable<Film> Films { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
