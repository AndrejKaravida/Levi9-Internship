using System;
using System.Collections.Generic;
using System.Linq;

namespace Code9.Amazon.WebAPI.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        { 
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            PageSize = pageSize;
            TotalCount = source.Count();
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(source.Count() / (double)pageSize);
            this.AddRange(items);
        }
    }
}
