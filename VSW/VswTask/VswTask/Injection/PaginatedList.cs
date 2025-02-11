﻿using Microsoft.EntityFrameworkCore;

namespace VswTask.Injection
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get;private set; }
        public int PageTotal { get;private set; }

        public PaginatedList(List<T> item, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageTotal = (int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(item);
        }

        public bool HasPreviousPage
        {
            get 
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < PageTotal);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
