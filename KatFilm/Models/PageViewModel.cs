using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatFilm.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; } //номер текущей страницы
        public int TotalPages { get; private set; } // общее количество страниц

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
         public bool HasPreviousPage
        {  // true - до текущей стр. нет страниц
            get
            {
                return (PageNumber > 1);
            }
        }
        public bool HasNextPage
        {  //true после текущей стр. есть страницы
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
